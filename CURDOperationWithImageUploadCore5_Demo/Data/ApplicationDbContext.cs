using CURDOperationWithImageUploadCore5_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace CURDOperationWithImageUploadCore5_Demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }
        public DbSet<Inventories> Inventory { get; set; }
    }
}
