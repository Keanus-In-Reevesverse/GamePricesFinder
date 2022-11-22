using GamePriceFinder.Database;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using Microsoft.EntityFrameworkCore;
using Windows.Devices.HumanInterfaceDevice;

namespace GamePriceFinder.Repositories
{
    /// <summary>
    /// Repository for History class, implements IRepository of History type.
    /// </summary>
    public class HistoryRepository : IRepository<History>
    {
        public HistoryRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public DatabaseContext DatabaseContext { get; }

        public void AddMany(List<History> entities)
        {
            throw new NotImplementedException();
        }

        public void AddOne(History history)
        {
            //var local = DatabaseContext.Set<History>().Local.FirstOrDefault(
            //    entry => 
            //    entry.GameIdentifier == history.GameIdentifier &&
            //    entry.StoreIdentifier == history.StoreIdentifier &&
            //    entry.ChangeDate == history.ChangeDate);

            var h = DatabaseContext.History.FirstOrDefault(entry => 
                entry.GameIdentifier == history.GameIdentifier &&
                entry.StoreIdentifier == history.StoreIdentifier &&
                entry.ChangeDate == history.ChangeDate);

            if (h != null)
            {
                return;
            }

            DatabaseContext.ChangeTracker.Clear();

            DatabaseContext.Entry(history).State = Microsoft.EntityFrameworkCore.EntityState.Detached;


            DatabaseContext.History.Add(history);
            DatabaseContext.SaveChanges();
        }

        public void EditOne(History entity)
        {
            throw new NotImplementedException();
        }

        public List<History> FindAll()
        {
            return DatabaseContext.History.AsNoTracking().ToList();
        }

        public History FindByGameId(int gameId)
        {
            History history;
            try
            {
                history = DatabaseContext.History.First(h => h.GameIdentifier == gameId);
            }
            catch (Exception e)
            {
                return null;
            }

            return history;
        }

        public History FindOneByName(string name)
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

        public void Update(History entity)
        {
            throw new NotImplementedException();
        }
    }
}
