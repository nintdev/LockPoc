using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using LockPoc.Core;
using LockPoc.Data;
using LockPoc.Data.Context;
using LockPoc.Data.Core;
using LockPoc.Data.Repositories;
using LockPoc.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LockPOC
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider;
        
        public static async Task Main(string[] args)
        {
            _serviceProvider = CreateServices();

            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                await UpdateDatabase(scope.ServiceProvider);
                await GetNumbers();
            }
        }

        private static async Task GetNumbers()
        {
            var random = new Random();
            var appSettings = _serviceProvider.GetRequiredService<IConfigurationProvider>();
            var numberService = _serviceProvider.GetRequiredService<INumberService>();
            var numberOfThreads = appSettings.GetValue<int>("NumberOfThreads");
            
            var salesDocumentTasks = Enumerable.Range(0, numberOfThreads)
                .Select(i => Task.Delay(random.Next(0, 500)).ContinueWith(t => numberService.GetNewSaleDocumentNumberAsync(), TaskContinuationOptions.LongRunning));
            
            var salesDocumentTaskResults = await Task.WhenAll(salesDocumentTasks);
            
            Debug.Assert(salesDocumentTaskResults.Select(tr => tr.Result).Distinct().Count() == numberOfThreads);
        }
        
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Registrations
                .AddSingleton<IConfigurationProvider, AppSettingsConfigurationProvider>()
                .AddSingleton<IDatabase, Database>()
                .AddSingleton<IContext, DapperContext>()
                .AddTransient<INumberRepository, NumberRepository>()
                .AddSingleton<INumberService, NumberService>()
                // FluentMigration
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString)
                    .ScanIn(typeof(Database).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build!
                .BuildServiceProvider(false);
        }
        
        private static async Task UpdateDatabase(IServiceProvider serviceProvider)
        {
            var database = _serviceProvider.GetRequiredService<IDatabase>();
            var databaseName = _serviceProvider.GetRequiredService<IConfigurationProvider>().GetString("DatabaseName");

            //await database.DropDatabaseAsync(databaseName);
            
            await database.CreateDatabaseAsync(databaseName);
            
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.ListMigrations();
            runner.MigrateUp();
            //runner.MigrateDown(202206020001);
        }
    }
}