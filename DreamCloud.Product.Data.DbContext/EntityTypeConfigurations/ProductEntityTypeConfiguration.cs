using System;
using System.Collections.Generic;
using System.Text;
using DreamCloud.Product.Data.DbContext.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCloud.Product.Data.DbContext.EntityTypeConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Entities.Product> builder)
        {
            builder.CreateEntityBaseProperties<Entities.Product, int>();

            // Append other configuration when needed.
        }
    }
}
