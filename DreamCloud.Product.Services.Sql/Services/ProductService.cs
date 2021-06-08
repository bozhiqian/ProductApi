using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamCloud.Product.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DreamCloud.Product.Services.Sql.Services
{
    public interface IProductService
    {
        Task<List<Data.Entities.Product>> GetProductsAsync();
        Data.Entities.Product GetProduct(int id);

        Task<Data.Entities.Product> AddProductAsync(Data.Entities.Product product);
    }

    public class ProductService : IProductService
    {
        private readonly ProductContext _context;
        private readonly ILogger<ProductService> _logger; // Not used for this demo so far.

        public ProductService(ProductContext productContext, ILogger<ProductService> logger)
        {
            _context = productContext;
            _logger = logger;
        }

        public virtual async Task<List<Data.Entities.Product>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products;
        }

        public virtual Data.Entities.Product GetProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            return product;
        }

        public virtual async Task<Data.Entities.Product> AddProductAsync(Data.Entities.Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product), "A product is required");

           _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
