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
        public Game FindOneByName(string name)
        {
            Game game = null;
            try
            {
                game =  DatabaseContext.Games.First(g => g.Name == name);
            }
            catch (Exception e)
            {


            }

            return game;
        }

        public int GetId(string gameName)
        {
            return DatabaseContext.Games.First(g => g.Name.Equals(gameName)).GameId;
        }

        public void Update(Game game)
        {
            var databaseGame = DatabaseContext.Games.First(g => g.GameId == game.GameId);
            databaseGame.Video = game.Video;
            databaseGame.Image = game.Image;
            DatabaseContext.Entry(databaseGame).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            DatabaseContext.SaveChanges();
        }
    }
}
