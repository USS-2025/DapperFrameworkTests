using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Async;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

using OracleTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace OracleTests
{
    internal class Program
    {
        const string SERILOG_OUTPUT_TEMPLATE =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{ThreadId:000}] [{Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}";

        static async Task<int> Main(string[] args)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environments.Development);

            var configBuilder = new ConfigurationBuilder();
            IConfigurationRoot configRoot = configBuilder.Build();

            IHostApplicationBuilder hostAppBuilder;
            // Deprecated:
            // HostApplicationBuilder hostAppBuilder = Host.CreateApplicationBuilder(args);
            // Deprecated:
            // IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);
            IHostBuilder hostBuilder = webAppBuilder.Host;
            IWebHostBuilder webHostBuilder = webAppBuilder.WebHost;

            // appBuilder = hostAppBuilder;
            hostAppBuilder = webAppBuilder;
            IConfigurationManager configManager = hostAppBuilder.Configuration;

            configManager.AddCommandLine(args);
            configManager.AddConfiguration(configRoot);
            configManager.AddEnvironmentVariables(prefix: "ASPNETCORE_");
            configManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configManager.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            webHostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(context.HostingEnvironment.ContentRootPath);
            });

            webHostBuilder.CaptureStartupErrors(true);
            webHostBuilder.UseSetting("detailedErrors", "true");

            // Deprecated:
            // webHostBuilder.UseStartup<Startup>();
            // Deprecated:
            // webHostBuilder.UseEnvironment(Environments.Development);

            IServiceCollection services = webAppBuilder.Services;

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Application", webAppBuilder.Environment.ApplicationName)
                    .Enrich.WithProperty("Environment", webAppBuilder.Environment.EnvironmentName)
                    .Enrich.WithThreadId()
                    .WriteTo.Async(a => a.Console(outputTemplate: SERILOG_OUTPUT_TEMPLATE))
                    .WriteTo.Async(a => a.Debug(outputTemplate: SERILOG_OUTPUT_TEMPLATE))
                    .WriteTo.Async(a => a.File("logs/log.txt", outputTemplate: SERILOG_OUTPUT_TEMPLATE, rollingInterval: RollingInterval.Day))
                    .WriteTo.Async(a => a.Seq(serverUrl: "http://localhost:5341")) // Standard-Port von Seq
                    .CreateLogger());
            });

            services.Configure<DatabaseOptions>(webAppBuilder.Configuration.GetSection(nameof(DatabaseOptions)));


            WebApplication webApp = webAppBuilder.Build();

            webApp.MapGet("/", (HttpContext context) =>
            {
                var dbOptions = context.RequestServices.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                IResult result = Results.Json(new
                {
                    //V = "DB-Konfig für Oracle:",
                    dbOptions.UserName,
                    dbOptions.Password,
                    dbOptions.Host,
                    dbOptions.Port,
                    dbOptions.ServiceName
                });

                return result;
            });

            IServiceProvider serviceProvider = webApp.Services;
            IWebHostEnvironment env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

            IServiceProvider scopedProvider = scope.ServiceProvider;

            DatabaseOptions dbOptions = scopedProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

           string connectionString = $"Data Source=" +
                "(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                $"(HOST={dbOptions.Host})" +
                $"(PORT={dbOptions.Port})" +
                "))" +
                "(CONNECT_DATA=(SERVER=DEDICATED)" +
                $"(SERVICE_NAME={dbOptions.ServiceName})" +
                "));" +
                $"User Id={dbOptions.UserName};" +
                $"Password={dbOptions.Password};";

            // Connect to the database 
            await using (var connection = new OracleConnection(connectionString))
            {
                await connection.OpenAsync();

                // Create a query that retrieves all books with an author name of "John Smith"    
                var sql = "SELECT * FROM Books WHERE Author=:authorName";
                // Use the Query method to execute the query and return a list of objects    
                //var books = connection.Query<Book>(sql, new { authorName = "John Smith" }).ToList();

                await connection.CloseAsync();
            }



            try
            {
                Assembly ass = Assembly.GetEntryAssembly();
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(ass.Location);

                var assVersion = ass.GetName().Version;
                string fileVersion = fileVersionInfo.FileVersion;
                string productVersion = fileVersionInfo.ProductVersion;

                Log.Information("Starting web host of {Application} ({Environment}), Version {Version} started successfully.",
                    env.ApplicationName, env.EnvironmentName, assVersion);
                
                await webApp.RunAsync();

                return 0;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
