using GamePriceFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").
                Build();

            var connString = configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;

            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genre { get; set; }
    }
}
