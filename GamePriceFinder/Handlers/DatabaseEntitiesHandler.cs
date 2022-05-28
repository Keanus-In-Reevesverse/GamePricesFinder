using GamePriceFinder.Models;

namespace GamePriceFinder.Handlers
{
    /// <summary>
    /// Class the unifies all the models to facilitate the database access.
    /// </summary>
    public class DatabaseEntitiesHandler
    {
        public DatabaseEntitiesHandler(Game game, GamePrices gamePrices, History history, Genre genre)
        {
            Game = game;
            GamePrices = gamePrices;
            History = history;
            Genre = genre;
        }

        /// <summary>
        /// Game in the handler.
        /// </summary>
        public Game Game { get; set; }
        /// <summary>
        /// Gameprices in the handler.
        /// </summary>
        public GamePrices GamePrices { get; set; }
        /// <summary>
        /// History in the handler.
        /// </summary>
        public History History { get; set; }
        /// <summary>
        /// Genre in the handler.
        /// </summary>
        public Genre Genre { get; }
    }
}
