using doudizhuServer.Models;
using Microsoft.EntityFrameworkCore;

namespace doudizhuServer
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<UserModel> UserDbSet { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {

        }
    }
}
