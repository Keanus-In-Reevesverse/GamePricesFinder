using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class Game : IGame
    {
        public Game (string name, Store store)
        {
            Name = name;
            Store = store;
            GameData = new GameData();
            History = new History();
        }

        public string Name { get; set; }

        public Store Store { get; private set; }

        public int GameId { get; set; }

        public GameData GameData { get; set; }

        public History History { get; set; }

        public Genre Genre { get; set; }
    }
}
