using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}