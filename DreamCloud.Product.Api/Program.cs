using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamCloud.Product.Data.DbContext;
using DreamCloud.Product.Services.Sql.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DreamCloud.Product.Api
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // migrate the database.  Best practice = in Main, using service scope
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var seedData = serviceProvider.GetRequiredService<ISeedData>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = serviceProvider.GetService<ProductContext>();
                    if (context != null) await context.Database.EnsureCreatedAsync();

                    if (_configuration != null && _configuration["ApplicationOptions:SeedProducts"].ToLower() == "true")
                    {
                        var c = await seedData.SeedPartnersAsync(context);
                        logger.LogInformation($"Total products {c} has been imported into product table.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding data to the database.");
                    throw;
                }
            }

            // run the web app
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, builder) =>
                    {
                        var config = builder.Build();
                        _configuration = config;
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
