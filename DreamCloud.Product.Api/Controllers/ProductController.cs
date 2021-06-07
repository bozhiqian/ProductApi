using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamCloud.Product.Common.Infrastructure.Services;
using DreamCloud.Product.Services.Sql.Services;
using Microsoft.AspNetCore.Http;

namespace DreamCloud.Product.Api.Controllers
{
    /// <summary>
    /// Product Api
    /// </summary>
    [ApiController]
    [Route("[controller]s")]
    //[ApiExplorerSettings(GroupName = "Product Catalog")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger;
        }

        /// <summary>
        /// Get All Products regardless of which bundle it is belong to.
        /// </summary>
        /// <returns>Product List</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceErrorInformation), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Models.Client.Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                if (products == null || !products.Any())
                    return NotFound();

                var productsForClient = products.Select(p => new Models.Client.Product()
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                }).ToList();

                return Ok(productsForClient);
            }
            catch (Exception ex)
            {
                return NotFound(new ServiceErrorInformation()
                {
                    Description = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get product by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceErrorInformation), StatusCodes.Status404NotFound)]
        public ActionResult<Models.Client.Product> GetProduct(int id)
        {
            try
            {
                var product = _productService.GetProduct(id);
                if (product == null)
                    return NotFound();

                var productForClient = new Models.Client.Product()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };

                return Ok(productForClient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new ServiceErrorInformation()
                {
                    Description = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }

        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceErrorInformation), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Models.Client.Product>> AddProduct(Models.Client.Product product)
        {
            try
            {
                var newProductEntity = new Data.Entities.Product()
                { Name = product.Name, Description = product.Description, Price = product.Price };

                var newProduct = await _productService.AddProductAsync(newProductEntity);

                return CreatedAtAction("GetProduct", new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return UnprocessableEntity(new ServiceErrorInformation()
                {
                    Description = ex.Message,
                    Timestamp = DateTime.UtcNow
                }); //Caution: never return error details to client in production. Logging should be used.
            }
        }
    }
}
