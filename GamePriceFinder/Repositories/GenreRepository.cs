using GamePriceFinder.Database;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        public GenreRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public DatabaseContext DatabaseContext { get; }

        public void AddMany(List<Genre> entities)
        {
            DatabaseContext.Genre.AddRange(entities);
        }

        public void AddOne(Genre entity)
        {
            try
            {
                entity.Description = "Adventure";
                DatabaseContext.Genre.Add(entity);
                DatabaseContext.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void EditOne(Genre entity)
        {
            throw new NotImplementedException();
        }

        public Genre FindOne(int id)
        {
            throw new NotImplementedException();
        }
    }
}
