using GamePriceFinder.Database;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Repositories
{

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

        public void AddOne(GamePrices entity)
        {
            throw new NotImplementedException();
        }

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

        public void Update(GamePrices gamePrice)
        {

            DatabaseContext.ChangeTracker.Clear();
            DatabaseContext.Entry(gamePrice).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            DatabaseContext.GamePrices.Update(gamePrice);
            DatabaseContext.SaveChanges();
            //var dbGamePrice = DatabaseContext.GamePrices.First(gp => gp.GameId == entity.GameId);
            //dbGamePrice.CurrentPrice = entity.CurrentPrice;
            //DatabaseContext.Entry(dbGamePrice).State = EntityState.Modified;
            //DatabaseContext.SaveChanges();
        }

        GamePrices IRepository<GamePrices>.FindByGameId(int gameId)
        {
            GamePrices dbGamePrices = null;
            try
            {
                dbGamePrices = DatabaseContext.GamePrices.AsNoTracking().First(gp => gp.GameId == gameId);
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

        public List<GamePrices> FindAll()
        {
            return DatabaseContext.GamePrices.AsNoTracking().ToList();
        }
    }
}
