using GamePriceFinder.Database;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Repositories
{
    /// <summary>
    /// Repository for Game class, implements IRepository of Game type.
    /// </summary>
    public class GameRepository : IRepository<Game>
    {
        public GameRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public DatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Adds many rows to database.
        /// </summary>
        /// <param name="entities"></param>
        public void AddMany(List<Game> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds one row to the database.
        /// </summary>
        /// <param name="entity"></param>
        public void AddOne(Game entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates one row in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void EditOne(Game entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects one row from the database matching the name of the game.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Game FindOne(string name)
        {
            Game a = null;
            try
            {
                a =  DatabaseContext.Games.First(g => g.Name == name);

            }
            catch (Exception e)
            {


            }

            return a;
        }
    }
}
