using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using FluentValidation;

namespace DreamCloud.Product.Models.Client
{
    /// <summary>
    /// Product for sale.
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductToAdd
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductToAddValidator<T> : AbstractValidator<T> where T : ProductToAdd
    {
        public ProductToAddValidator()
        {
            RuleFor(product => product.Name).NotNull();
        }
    }
}
