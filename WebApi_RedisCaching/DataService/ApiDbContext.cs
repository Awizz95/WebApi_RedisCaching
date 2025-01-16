using Microsoft.EntityFrameworkCore;
using WebApi_RedisCaching.Models;

namespace WebApi_RedisCaching.DataService
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
    }
}
