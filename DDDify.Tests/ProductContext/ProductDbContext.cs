using DDDify.Tests.ProductContext.Aggregates;

using Microsoft.EntityFrameworkCore;

namespace DDDify.Tests.ProductContext
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
