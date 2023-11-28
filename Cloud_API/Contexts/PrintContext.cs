using Cloud_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud_API.Contexts
{
    public class PrintContext : DbContext
    {

        public PrintContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Receipt> Receipts { get; set; } = null!;
    }
}
