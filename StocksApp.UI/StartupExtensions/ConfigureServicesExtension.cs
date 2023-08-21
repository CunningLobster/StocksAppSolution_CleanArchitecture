using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace StocksApp
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();

            services.AddScoped<IBuyOrderRepository, BuyOrderRepository>();
            services.AddScoped<ISellOrderRepository, SellOrderRepository>();
            services.AddScoped<IFinhubRepository, FinhubRepository>();
            services.AddScoped<IFinnhubService, FinnhubService>();
            services.AddScoped<IStockBuyService, StockBuyService>();
            services.AddScoped<IStockSellService, StockSellService>();

            services.Configure<TradingOptions>(builder.Configuration.GetSection(TradingOptions.TradingOptionsSection));

            services.AddHttpLogging(options => {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            //Connection to the database
            string connectionString = String.Empty;
            if (builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Test")
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            else
            {
                // Use connection string provided at runtime by Fly.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];
                var updatedHost = pgHost.Replace("flycast", "internal");

                connectionString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
            }
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

    }
}
