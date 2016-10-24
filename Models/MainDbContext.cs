using Microsoft.EntityFrameworkCore;

namespace AKK.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {}
    }
}