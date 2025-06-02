using Dapper.Extensions.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Dommel;
using DataModels.Models;
using System.Diagnostics;
using Dapper.Extensions.SQL;
using System.Data;
using Dapper;

namespace OracleTestsDapper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Serilog konfigurieren
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Seq("http://localhost:5341") // Standard-Port von Seq
                .CreateLogger();

            // DI Container konfigurieren
            var services = new ServiceCollection();

            services.AddLogging(builder =>
             {
                 builder.ClearProviders(); // Entfernt Microsoft Console Logger
                 builder.AddSerilog();     // Nutzt Serilog stattdessen
             });

            //For Oracle
            services.AddDapperForOracle();









            // Provider erstellen
            var serviceProvider = services.BuildServiceProvider();



            string connectionString = $"Data Source=" +
                 "(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                 $"(HOST=192.168.0.200)" +
                 $"(PORT=1521)" +
                 "))" +
                 "(CONNECT_DATA=(SERVER=DEDICATED)" +
                 $"(SERVICE_NAME=freepdb1)" +
                 "));" +
                 $"User Id=system;" +
                 $"Password=oracle;";

            try
            {
                // Connect to the database 
                await using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Create a query that retrieves all books with an author name of "John Smith"    
                    const string sql = "SELECT * FROM Help";

                    await using (var command = new OracleCommand(sql, connection))
                    {
                        await using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // which column index?
                                //int ordinal = reader.GetOrdinal("INFO");
                                //string? content = reader.IsDBNull("INFO") ? null: reader.GetString("INFO");
                            }
                        }
                    }

                    // Use the Query method to execute the query and return a list of objects    
                    //helpLines = await connection.GetAllAsync<Help>();
                    var helpLines = connection.Query<Help>(sql, new { Topic = "@@" }).ToList();

                    string HelpText = string.Join(Environment.NewLine, helpLines.Select(hl => hl.Info));
                    Console.WriteLine(HelpText);
                    Log.Verbose("Help-Text: {HelpText}", HelpText);

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Console.WriteLine(ex);
                Log.Error(ex, $"{ex}");
            }

        }
    }
}
