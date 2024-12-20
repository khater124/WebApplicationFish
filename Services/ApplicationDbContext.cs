using Microsoft.EntityFrameworkCore;
using WebApplicationFish.Models;

namespace WebApplicationFish.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Fish> Fishes { get; set; }
    }
}
