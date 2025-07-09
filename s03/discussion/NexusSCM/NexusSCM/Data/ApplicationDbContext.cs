using Microsoft.EntityFrameworkCore;
using Nexus.Core;

namespace Nexus.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
