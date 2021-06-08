using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamCloud.Product.Data.DbContext;
using DreamCloud.Product.Data.DbContext.Extensions;
using Microsoft.Extensions.Logging;

namespace DreamCloud.Product.Services.Sql.Services
{
    public interface ISeedData
    {
        Task<int> SeedProductsAsync(ProductContext context);
    }

    public class SeedData : ISeedData
    {
        private readonly ILogger<SeedData> _logger;

        public SeedData(ILogger<SeedData> logger) 
        {
            _logger = logger;
        }

        public async Task<int> SeedProductsAsync(ProductContext context)
        {
            if (context.Products.Any())
            {
                return 0; // Not overwrite existing data.
            }

            try
            {
                var products = new List<Data.Entities.Product>
                {
                    new Data.Entities.Product()
                    {
                        Name = "Product1",
                        Description = "Product1 Description",
                        Price = .99m
                    },
                    new Data.Entities.Product()
                    {
                        Name = "Product2",
                        Description = "Product2 Description",
                        Price = 2.99m
                    },
                    new Data.Entities.Product()
                    {
                        Name = "Product3",
                        Description = "Product3 Description",
                        Price = 3.99m
                    }
                };

                context.Products.AddOrUpdate(products);

                var count = await context.SaveChangesAsync();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(default, ex, ex.Message);
            }

            return 0;
        }

    }
}
