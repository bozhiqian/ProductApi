using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamCloud.Product.Api.Controllers;
using DreamCloud.Product.Models.Client;
using DreamCloud.Product.Services.Sql.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace DreamCloud.Product.Api.Test
{
    public class ProductControllerTest
    {
        private ProductController _productController;
        private readonly Mock<ProductService> _mockProductService;
        private readonly ProductToAddValidator<ProductToAdd> _validator;

        public ProductControllerTest()
        {
            _mockProductService = new Mock<ProductService>(null, new NullLogger<ProductService>());
            _validator = new ProductToAddValidator<ProductToAdd>();
        }

        [Fact]
        public async Task Can_get_products()
        {
            _mockProductService.Setup(s => s.GetProductsAsync()).Returns(Task.FromResult(ProductsTestData()));
            _productController = new ProductController(_mockProductService.Object, new NullLogger<ProductController>(), _validator);

            var result = await _productController.GetProducts();
            var okObjectResult = (OkObjectResult)result.Result;
            var products = okObjectResult.Value as List<Models.Client.Product>;

            Assert.NotNull(products);
            Assert.Equal(3, products.Count);
            Assert.Contains(products, p => p.Name.Equals("Product1"));
        }

        [Fact]
        public async Task Can_get_product_by_id()
        {
            _mockProductService.Setup(s => s.GetProduct(It.IsAny<int>())).Returns(new Product.Data.Entities.Product()
            {
                Name = "Product1",
                Description = "Product1 Description",
                Price = .99m
            });

            _productController = new ProductController(_mockProductService.Object, new NullLogger<ProductController>(), _validator);

            var result = _productController.GetProduct(1);
            var okObjectResult = (OkObjectResult)result.Result;
            var product = okObjectResult.Value as Models.Client.Product;

            Assert.NotNull(product);
            Assert.Equal("Product1", product.Name);
        }

        [Fact]
        public async Task Can_add_product()
        {
            _mockProductService.Setup(s => s.AddProductAsync(It.IsAny<Product.Data.Entities.Product>())).Returns(Task.FromResult(new Product.Data.Entities.Product()
            {
                Name = "Product4",
                Description = "Product4 Description",
                Price = .99m
            }));

            _productController = new ProductController(_mockProductService.Object, new NullLogger<ProductController>(), _validator);

            var productToAdd = new ProductToAdd()
            {
                Name = "Product4",
                Description = "Product4 Description",
                Price = .99m
            };

            var result = await _productController.AddProductAsync(productToAdd);
            var actionResult = (CreatedAtActionResult)result.Result;
            var product = actionResult.Value as Models.Client.Product;

            Assert.NotNull(product);
            Assert.Equal("Product4", product.Name);
        }

        [Fact]
        public async Task Add_product_failed_without_name()
        {
            _mockProductService.Setup(s => s.AddProductAsync(It.IsAny<Product.Data.Entities.Product>())).Returns(Task.FromResult(new Product.Data.Entities.Product()
            {
                Name = "Product5",
                Description = "Product5 Description",
                Price = .99m
            }));

            _productController = new ProductController(_mockProductService.Object, new NullLogger<ProductController>(), _validator);

            var productToAdd = new ProductToAdd()
            {
                Description = "Product5 Description",
                Price = .99m
            };

            var result = await _productController.AddProductAsync(productToAdd);
            var actionResult = (ObjectResult)result.Result;

            Assert.IsType<ValidationProblemDetails>(actionResult.Value);
            var error = actionResult.Value as ValidationProblemDetails;

            Assert.NotNull(error);
            Assert.True(error.Errors.Any());
            Assert.Equal("Name", error.Errors.First().Key);
        }

        private List<Product.Data.Entities.Product> ProductsTestData()
        {
            var products = new List<Product.Data.Entities.Product>
            {
                new Product.Data.Entities.Product()
                {
                    Name = "Product1",
                    Description = "Product1 Description",
                    Price = .99m
                },
                new Product.Data.Entities.Product()
                {
                    Name = "Product2",
                    Description = "Product2 Description",
                    Price = 2.99m
                },
                new Product.Data.Entities.Product()
                {
                    Name = "Product3",
                    Description = "Product3 Description",
                    Price = 3.99m
                }
            };

            return products;
        }
    }
}
