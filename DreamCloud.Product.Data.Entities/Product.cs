using System;
using System.Runtime.CompilerServices;

namespace DreamCloud.Product.Data.Entities
{
    /// <summary>
    /// Product
    /// </summary>
    public class Product : IdentityEntityBase<int>
    {
        
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        // Navigation property of EF.
    }
}
