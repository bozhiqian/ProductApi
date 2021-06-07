using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DreamCloud.Product.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCloud.Product.Data.DbContext.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void CreateEntityBaseProperties<TEntity, TProperty>(
            this EntityTypeBuilder<TEntity> entity) where TEntity : IdentityEntityBase<TProperty>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName($"{typeof(TEntity).Name}Id").UseIdentityColumn();
            entity.Property(e => e.CreatedDateUtc).HasDefaultValueSql("(getutcdate())");
        }
    }
}
