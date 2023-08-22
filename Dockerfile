#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StocksApp.UI/StocksApp.UI.csproj", "StocksApp.UI/"]
COPY ["StocksApp.Core/StocksApp.Core.csproj", "StocksApp.Core/"]
COPY ["StocksApp.Infrastructure/StocksApp.Infrastructure.csproj", "StocksApp.Infrastructure/"]
RUN dotnet restore "StocksApp.UI/StocksApp.UI.csproj"
COPY . .
WORKDIR "/src/StocksApp.UI"
RUN dotnet build "StocksApp.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StocksApp.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksApp.UI.dll", "--environment=Production"]