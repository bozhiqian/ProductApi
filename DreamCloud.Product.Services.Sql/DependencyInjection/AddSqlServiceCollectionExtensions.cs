using DreamCloud.Product.Data.DbContext;
using DreamCloud.Product.Services.Sql.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DreamCloud.Product.Services.Sql.DependencyInjection
{
    public static class AddSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureSqlDatabaseServices(this IServiceCollection services, string partnerSqlDbConnectionString)
        {
            services.AddScoped<IProductService>(s => new ProductService(s.GetRequiredService<ProductContext>(), s.GetRequiredService<ILogger<ProductService>>()));
            services.AddSingleton<ISeedData>(s => new SeedData(s.GetRequiredService<ILogger<SeedData>>()));

            return services;
        }
    }
}
