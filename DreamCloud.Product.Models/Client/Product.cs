using System;
using System.Collections.Generic;
using System.Text;

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
}
