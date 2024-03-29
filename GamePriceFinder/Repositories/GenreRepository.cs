﻿using GamePriceFinder.Database;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

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

        /// <summary>
        /// Selects one row from the database matching the name of the genre.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Genre FindOne(string name)
        {
            throw new NotImplementedException();
        }
    }
}
