using Microsoft.EntityFrameworkCore;

namespace doudizhuServer
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {

        }
    }
}