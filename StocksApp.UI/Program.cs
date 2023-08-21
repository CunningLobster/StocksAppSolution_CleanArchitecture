using CRUDExample.Middleware;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using StocksApp;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((builderContext, services, configuration) =>
{
    configuration.ReadFrom.Configuration(builderContext.Configuration).ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseStaticFiles();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Migrate database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.UseHttpLogging();

if(!builder.Environment.IsEnvironment("Test"))
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot");

app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { };
