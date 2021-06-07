using System;
using System.Threading;
using System.Threading.Tasks;
using DreamCloud.Product.Data.DbContext.EntityTypeConfigurations;
using DreamCloud.Product.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DreamCloud.Product.Data.DbContext
{
    public class ProductContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ProductContext(DbContextOptions options) : base(options)
        {
            base.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds);
        }

        #region Enities

        public virtual DbSet<Entities.Product> Products { get; set; }
        

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = this.Database.GetDbConnection().ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IdentityEntityBase<int>>()) // Todo... for rest of other IdentityEntityBase<T>.
            {
                var entity = entry.Entity;
                var utcNow = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        // No work for this now.
                        break;
                    case EntityState.Deleted:
                        // No work for this now.
                        break;
                    case EntityState.Modified:
                        entity.ModifiedDateUtc = utcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
