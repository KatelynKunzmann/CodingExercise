# Investment Performance Web API

## Setup

1. Ensure you have .NET 9 installed
   `dotnet --version`

2. Navigate to the project dir
   `cd InvestmentPerformanceWebAPI`

3. Check if Migrations folder exists and is populated
   If empty, recreate with
   `dotnet ef migrations add InitialCreate`

4. Create/update the SQLite database
   `dotnet ef database update`

5. Restore packages and build
   `dotnet restore`
   `dotnet build`

6. Run the API
   `dotnet run`
   or if in solution root directory:
   `dotnet run --project InvestmentPerformanceWebAPI`

   - Can also run with https:
     `dotnet run --launch-profile https`
     or if in solution root directory:
     `dotnet run --project InvestmentPerformanceWebAPI --launch-profile https`

7. Once server is running, navigate to swagger path for API UI
   <http://localhost:5048/swagger>
   For https: <https://localhost:7058/swagger/>

   - Trust the dev certificate

8. A new db file called investment.db should exist and contain dummy seeded data that you can enter into Swagger UI to see responses:
   User 1: 2 investments (InvestmentIds 1, 2)
   User 2: 1 investment (InvestmentId 3)

## Example API Responses

GET /api/v1/users/1/investments

```json
[
  { "investmentId": 1, "name": "Apple Inc." },
  { "investmentId": 2, "name": "Tesla Motors" }
]
```

GET /api/v1/users/1/investments/1

```json
{
  "investmentId": 1,
  "name": "Apple Inc.",
  "ticker": "AAPL",
  "numberOfShares": 10,
  "costBasisPerShare": 120,
  "currentPrice": 175,
  "currentValue": 1750,
  "term": "Long Term",
  "totalGainLoss": 550
}
```

## Running tests

1. From the solution root directory, run
   `dotnet test`

2. Expected output:
   `Passed!  - 4 passed, 0 failed, 0 skipped`

3. Test Coverage:
   - GetInvestmentsForUser_ReturnsInvestments_WhenUserExists -> Returns the seeded investments for a valid user
   - GetInvestmentDetails_ReturnsCorrectDetails_WhenInvestmentExists -> Returns details for specific investment
   - GetInvestmentsForUser_ReturnsNotFound_WhenUserDoesNotExist -> Returns 404 for a not found user
   - GetInvestmentDetails_ReturnsNotFound_WhenInvestmentDoesNotExist -> Returns 404 for a not found investment

## Notes

- Tech Stack:
  - C# and .NET 9 (.NET 9.0.306 -> my version)
  - ASP.NET Core Web API - controller based
  - Database: SQLite with EF Core ORM
  - Testing: xUnit and some Moq
  - Swagger and OpenAPI
- Project was made to be prod-ready but uses dummy seeded data for ease of use and an in memory database for testing
- Since this will be connected to a trading platform, decided to add Ticker field as well

## Assumptions/Tradeoffs

- Given the goal is to deliver a production-ready API within a few hours, I prioritized clean architecture (separating data, models, controllers), unit tests, basic security such as https, versioning in api path, and server-side and client-side logging was kept distinct from each other. If this was a longer term devloped/maintained project then some areas that could be implemented on are:
  - Global exception middleware for more fitting error handling and centralized logging
  - Data annotations for input validation and database constraints
  - Rate limiting
  - Response caching
  - Along with many other enhancements
