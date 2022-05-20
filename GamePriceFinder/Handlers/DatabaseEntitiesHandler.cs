using GamePriceFinder.Models;

namespace GamePriceFinder.Handlers
{
    public class DatabaseEntitiesHandler
    {
        public DatabaseEntitiesHandler(Game game, GamePrices gamePrices, History history)
        {
            Game = game;
            GamePrices = gamePrices;
            History = history;
        }

        public Game Game { get; set; }
        public GamePrices GamePrices { get; set; }
        public History History { get; set; }
    }
}
