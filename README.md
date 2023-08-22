# StocksApp
## Overview
This is a demo project which created to show my Asp.NET Core skills.
The project is a connector to [Finnhub Stock API](https://finnhub.io/). Nowadays it includes the functionality below:
- Explore page with list of available stocks from finnhub.io
- Live chart and price updates on the selected stock (but only when the market is LIVE i.e. usually 09:30 a.m. to 4 p.m. (ET))
- Place buy or sell order
- A dashboard to see order history of buy orders and sell orders
## Stack of technologies
### Platform
- C#, Asp.Net Core
### Additional frameworks and technologies
- Razor Syntax
- Entity Framework Core
- xUnit
- Moq
- AutoFixture
- Fluent Assertions
- Rotativa
- Serilog
## How To Launch
The project is published on [fly.io](https://fly.io/). Just follow the link https://stocksapp-clean.fly.dev
### For those who want to clone this project
After cloning the project be sure you've done all the stuff below:
#### Set User Secrets
Before running the application you should set the Finhub API Key in user secrets. In command line execute next commands:
```
dotnet user-secrets init --project "StocksApp.UI"
```
```
dotnet user-secrets set "FINNHUB_API_KEY" "<FinnhubKey>" --project "StocksApp.UI"
```
To get FinnhubKey value you should:
1. Create an account on [Finnhub](https://finnhub.io/)
2. At the main page follow to [Dashboard](https://finnhub.io/dashboard)
3. Get your API Key
#### Connect to a database
Also, for the correct operation of the service you should to connect to a Postgres Database. You can set the connection string in appsettings.Development.json file in "StocksApp.UI" directory. 

