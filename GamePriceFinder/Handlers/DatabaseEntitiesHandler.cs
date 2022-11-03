using GamePriceFinder.MVC.Models;

namespace GamePriceFinder.Handlers
{
    /// <summary>
    /// Class the unifies all the models to facilitate the database access.
    /// </summary>
    public class EntitiesHandler
    {
        public EntitiesHandler(Game game, GamePrices gamePrices, History history, Genre genre)
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
