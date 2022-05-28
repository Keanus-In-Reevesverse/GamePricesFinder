using GamePriceFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Database
{
    /// <summary>
    /// Declares the database tables in the code for database interaction.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<History>().HasNoKey();
        }

        /// <summary>
        /// Configures the connection string and defines the database to be used as MySQL.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").
                Build();

            var connString = configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;

            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        }

        /// <summary>
        /// Game table.
        /// </summary>
        public DbSet<Game> Games { get; set; }
        /// <summary>
        /// Genre table.
        /// </summary>
        public DbSet<Genre> Genre { get; set; }
        /// <summary>
        /// History table.
        /// </summary>
        public DbSet<History> History { get; set; }
        /// <summary>
        /// Game_Prices table.
        /// </summary>
        public DbSet<GamePrices> GamePrices { get; set; }
    }
}
