using GamePriceFinder.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamePrices>()
                .HasKey("GameId", "StoreId");

            modelBuilder.Entity<History>()
                .HasKey("GameIdentifier", "StoreIdentifier", "ChangeDate");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").
                Build();

            var connString = configuration.GetSection("MySqlConnection:MySqlConnectionString").Value;

            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString), options =>
            {
                options.EnableStringComparisonTranslations();
            });

            optionsBuilder.EnableSensitiveDataLogging(true);
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
