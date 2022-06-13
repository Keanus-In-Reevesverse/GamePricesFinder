using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Repositories
{
    /// <summary>
    /// Repository for GamePrices class, implements IRepository of GamePrices type.
    /// </summary>
    public class GamePricesRepository : IRepository<GamePrices>
    {
        /// <summary>
        /// Adds many rows to database.
        /// </summary>
        /// <param name="entities"></param>
        public void AddMany(List<GamePrices> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds one row to the database.
        /// </summary>
        /// <param name="entity"></param>
        public void AddOne(GamePrices entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates one row in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void EditOne(GamePrices entity)
        {
            throw new NotImplementedException();
        }

        public GamePrices FindOne(int id)
        {
            throw new NotImplementedException();
        }

        public int GetId(string gameName)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePrices>.AddMany(List<GamePrices> entities)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePrices>.AddOne(GamePrices entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<GamePrices>.EditOne(GamePrices entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects one row from the database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GamePrices IRepository<GamePrices>.FindOneByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
