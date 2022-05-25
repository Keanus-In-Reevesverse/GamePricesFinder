namespace GamePriceFinder.Database
{
    public static class DatabaseInitializer
    {
        public static void Initialize(DatabaseContext databaseContext)
        {
            databaseContext.Database.EnsureCreated();
        }
    }
}
