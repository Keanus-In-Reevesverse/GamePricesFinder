using GamePriceFinder.Database;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace GamePriceFinder.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        public GameRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public DatabaseContext DatabaseContext { get; }
        public void AddMany(List<Game> entities)
        {
            throw new NotImplementedException();
        }
        public void AddOne(Game entity)
        {
            DatabaseContext.Games.Add(entity);
            DatabaseContext.SaveChanges();
        }
        public void EditOne(Game entity)
        {
            throw new NotImplementedException();
        }

        public List<Game> FindAll()
        {
            List<Game> games;
            try
            {
                games = DatabaseContext.Games.AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                return null;
            }

            return games;
        }

        public Game FindByGameId(int gameId)
        {
            Game game;
            try
            {
                game = DatabaseContext.Games.AsNoTracking().First(g => g.GameId == gameId);
            }
            catch (Exception e)
            {
                return null;
            }

            return game;
        }
        public Game FindOneByName(string name)
        {
            Game game = null;
            try
            {
                game =  DatabaseContext.Games.AsNoTracking().First(g => g.Name == name);
            }
            catch (Exception e)
            {
                return null;
            }

            return game;
        }

        public int GetId(string gameName)
        {
            return DatabaseContext.Games.AsNoTracking().First(g => g.Name.Equals(gameName)).GameId;
        }

        public void Update(Game game)
        {
            var databaseGame = DatabaseContext.Games.First(g => g.GameId == game.GameId);
            databaseGame.Video = string.IsNullOrEmpty(game.Video) ? string.Empty : game.Video;
            databaseGame.Image = game.Image;
            DatabaseContext.Entry(databaseGame).State = EntityState.Modified;
            DatabaseContext.SaveChanges();
        }
    }
}
