using GamePriceFinder.Models;

namespace GamePriceFinder.Handlers
{
    public class DatabaseEntitiesHandler
    {
        public DatabaseEntitiesHandler(Game game, GamePrices gamePrices, History history, Genre genre)
        {
            Game = game;
            GamePrices = gamePrices;
            History = history;
            Genre = genre;
        }

        public Game Game { get; set; }
        public GamePrices GamePrices { get; set; }
        public History History { get; set; }
        public Genre Genre { get; }
    }
}
