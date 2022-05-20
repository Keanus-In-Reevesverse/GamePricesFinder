using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class Game : IGame
    {
        public Game (string name, StoresEnum store)
        {
            Name = name;
        }

        public int GenreId { get; set; }
        public int GameId { get; set; }
        public string Video { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Name { get; set; }
    }
}
