using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksAppTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services => {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if(descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });

            builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, IConfigurationBuilder config) =>
            {
                var newConfiguration = new Dictionary<string, string>() 
                {
                   { "FINNHUB_API_KEY", "chq4jv9r01qt7cgvstugchq4jv9r01qt7cgvstv0" } //add token value
                };

                config.AddInMemoryCollection(newConfiguration);
            });

        }
    }
}
