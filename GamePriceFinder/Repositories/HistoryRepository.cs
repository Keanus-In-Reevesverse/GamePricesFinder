﻿using GamePriceFinder.Database;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

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

        /// <summary>
        /// Inserts many rows in the database.
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddMany(List<History> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts only one row in database.
        /// </summary>
        /// <param name="history"></param>
        public void AddOne(History history)
        {
            DatabaseContext.History.Add(history);
            DatabaseContext.SaveChanges();
        }

        /// <summary>
        /// Updates data of one row in database.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void EditOne(History entity)
        {
            throw new NotImplementedException();
        }

        public List<History> FindAll()
        {
            throw new NotImplementedException();
        }

        public History FindByGameId(int gameId)
        {
            History history;
            try
            {
                history = DatabaseContext.History.First(h => h.GameId == gameId);
            }
            catch (Exception e)
            {
                return null;
            }

            return history;
        }

        /// <summary>
        /// Selects one row from database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
