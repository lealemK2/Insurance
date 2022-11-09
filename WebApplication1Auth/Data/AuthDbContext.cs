using Microsoft.EntityFrameworkCore;
using Insurance.Model;

namespace Insurance.Data
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set;}
    }
}
