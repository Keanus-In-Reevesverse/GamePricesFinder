using GamePriceFinder.Database;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Repositories
{
    /// <summary>
    /// Repository for GamePrices class, implements IRepository of GamePrices type.
    /// </summary>
    public class GamePricesRepository : IRepository<GamePrices>
    {
        public GamePricesRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public DatabaseContext DatabaseContext { get; }
        
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

        internal GamePrices FindByGameId(int gameId)
        {
            return DatabaseContext.GamePrices.First(gp => gp.GameId == gameId);
        }

        void IRepository<GamePrices>.AddOne(GamePrices entity)
        {
            DatabaseContext.GamePrices.Add(entity);
            DatabaseContext.SaveChanges();
        }

        public void Update(GamePrices entity)
        {
            var dbGamePrice = DatabaseContext.GamePrices.First(gp => gp.GameId == entity.GameId);
            dbGamePrice.CurrentPrice = entity.CurrentPrice;
            DatabaseContext.Entry(dbGamePrice).State = EntityState.Modified;
            DatabaseContext.SaveChanges();
        }

        GamePrices IRepository<GamePrices>.FindByGameId(int gameId)
        {
            GamePrices dbGamePrices = null;
            try
            {
                dbGamePrices = DatabaseContext.GamePrices.First(gp => gp.GameId == gameId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }

            return dbGamePrices;
        }
    }
}
