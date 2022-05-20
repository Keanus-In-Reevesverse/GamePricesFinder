using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        public void AddMany(List<Game> entities)
        {
            throw new NotImplementedException();
        }

        public void AddOne(Game entity)
        {
            throw new NotImplementedException();
        }

        public void EditOne(Game entity)
        {
            throw new NotImplementedException();
        }

        public Game FindOne(int id)
        {
            throw new NotImplementedException();
        }
    }
}
