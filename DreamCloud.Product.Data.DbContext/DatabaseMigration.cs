using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DreamCloud.Product.Data.DbContext
{
    public class DatabaseMigration
    {
        public static void Migrate(ProductContext context, ILogger logger = null)
        {
            try
            {
                if (context.Products.Any()) { return; }
                if (context.Products.Any()) { return; }

                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
