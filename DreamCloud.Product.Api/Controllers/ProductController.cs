using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamCloud.Product.Common.Infrastructure.Services;
using DreamCloud.Product.Models.Client;
using DreamCloud.Product.Services.Sql.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace DreamCloud.Product.Api.Controllers
{
    /// <summary>
    /// Product Api
    /// </summary>
    [ApiController]
    [Route("[controller]s")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IValidator<ProductToAdd> _validator;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IValidator<ProductToAdd> validator)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger;
            _validator = validator;
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
                var products = await _productService.GetProductsAsync();
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceErrorInformation), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Models.Client.Product>> AddProductAsync(Models.Client.ProductToAdd product)
        {
            try
            {
                // Validate product model.
                var validationResult = await _validator.ValidateAsync(product);
                if (!validationResult.IsValid)
                {
                    var validationFailures = new List<ValidationFailure>();

                    foreach (var failure in validationResult.Errors)
                    {
                        validationFailures.Add(new ValidationFailure(failure.PropertyName, failure.ErrorMessage));
                    }

                    return ThrowModelValidationError(validationFailures);
                }

                var newProductEntity = new Product.Data.Entities.Product()
                { Name = product.Name, Description = product.Description, Price = product.Price }; // Todo... Use AutoMapper.

                var newProduct = await _productService.AddProductAsync(newProductEntity);

                return CreatedAtAction("GetProduct", new { id = newProduct.Id }, new Models.Client.Product()
                {
                    ProductId = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price
                }); // Todo... Use AutoMapper.
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

        #region Validations

        protected ActionResult ThrowModelValidationError(List<ValidationFailure> validationFailures)
        {
            var validationResult = GetModelValidationError(validationFailures);

            return ValidationProblem(ModelState);
        }

        protected ValidationResult GetModelValidationError(List<ValidationFailure> validationFailures)
        {
            var validationResult = new ValidationResult(validationFailures);
            validationResult.AddToModelState(ModelState, null);

            return validationResult;
        }
        #endregion
    }
}
