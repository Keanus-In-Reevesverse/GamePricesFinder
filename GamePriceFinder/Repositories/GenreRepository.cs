using GamePriceFinder.Database;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Repositories
{
    /// <summary>
    /// Repository for Genre class, implements IRepository of Genre type.
    /// </summary>
    public class GenreRepository : IRepository<Genre>
    {
        public GenreRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        /// <summary>
        /// Database handler.
        /// </summary>
        public DatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Adds many rows to database.
        /// </summary>
        /// <param name="entities"></param>
        public void AddMany(List<Genre> entities)
        {
            DatabaseContext.Genre.AddRange(entities);
        }

        /// <summary>
        /// Adds one row to the database.
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// Updates one row in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void EditOne(Genre entity)
        {
            throw new NotImplementedException();
        }

        public List<Genre> FindAll()
        {
            throw new NotImplementedException();
        }

        public Genre FindByGameId(int gameId)
        {
            throw new NotImplementedException();
        }
        public Genre FindOneByName(string name)
        {
            return 
                DatabaseContext.Genre.AsNoTracking().FirstOrDefault(gen => gen.Description.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public int GetId(string gameName)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }

        public void Update(Genre entity)
        {
            throw new NotImplementedException();
        }
    }
}
