using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class Game : IGame
    {
        public Game (string name)
        {
            _name = name;
            GameData = new GameData();
            History = new History();
        }

        private string _name;
        public string Name { get => _name; set => _name = value; }
        public Store Store { get; set; }
        public int GameId { get; set; }
        public GameData GameData { get; set; }
        public History History { get; set; }
        public Genre Genre { get; set; }
    }
}
