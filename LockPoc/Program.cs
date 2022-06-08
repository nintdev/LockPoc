using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using LockPoc;
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
                await UpdateDatabase(scope);
                await GetNumbers(scope); // todo: cancellation
            }
        }

        private static async Task GetNumbers(IServiceScope scope)
        {
            var random = new Random();
            var appSettings = scope.ServiceProvider.GetRequiredService<IConfigurationProvider>();
            var numberService = scope.ServiceProvider.GetRequiredService<INumberService>();
            var numberOfThreads = appSettings.GetValue<int>("NumberOfThreads");

            var tasks = Enumerable.Range(0, numberOfThreads)
                .Select(i =>
                    Task.Delay(random.Next(0, 500)).ContinueWith(t => numberService.GetNewSaleDocumentNumberAsync(),
                        TaskContinuationOptions.LongRunning));
                    
            var taskResults = await Task.WhenAll(tasks);
            
            Debug.Assert(taskResults.Select(tr => tr.Result).Distinct().Count() == numberOfThreads);
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
                    .WithVersionTable(new VersionInfo())
                    .WithGlobalConnectionString(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString)
                    .ScanIn(typeof(Database).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build!
                .BuildServiceProvider(false);
        }
        
        private static async Task UpdateDatabase(IServiceScope scope)
        {
            var database = scope.ServiceProvider.GetRequiredService<IDatabase>();
            var databaseName = scope.ServiceProvider.GetRequiredService<IConfigurationProvider>().GetString("DatabaseName");

            //await database.DropDatabaseAsync(databaseName);
            
            await database.CreateDatabaseAsync(databaseName);
            
            // Instantiate the runner
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.ListMigrations();
            runner.MigrateUp();
            //runner.MigrateDown(202206020001);
        }
    }
}