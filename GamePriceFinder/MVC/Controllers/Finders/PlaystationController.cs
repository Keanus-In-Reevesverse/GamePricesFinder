using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;
using GamePriceFinder.MVC.Models.Responses;
using Genre = GamePriceFinder.MVC.Models.Genre;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    /// <summary>
    /// Represents the Playstation price finder, implements IPriceFinder.
    /// </summary>
    public class PlaystationController : IPriceFinder
    {
        public PlaystationController()
        {
            HttpHandler = new HttpController();
        }

        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Playstation Store.
        /// </summary>
        public HttpController HttpHandler { get; set; }

        private const string TRAILER = " trailer";

        /// <summary>
        /// Gets Playstation prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            Link[] responseGameList;
            try
            {
                responseGameList = await HttpHandler.GetToPsn(FormatName(gameName));
            }
            catch (Exception)
            {
                return null;
            }

            var entities = new List<DatabaseEntitiesHandler>();

            foreach (var responseGame in responseGameList)
            {
                if (responseGame.default_sku == null)
                {
                    continue;
                }

                if (responseGame.default_sku.name.Equals("Jogo Completo", StringComparison.CurrentCultureIgnoreCase) ||
                    responseGame.default_sku.name.Equals("Jogo", StringComparison.CurrentCultureIgnoreCase) ||
                    responseGame.default_sku.name.Equals("Jogo Completo e Conteúdo Complementar", StringComparison.CurrentCultureIgnoreCase))
                {
                    var title = responseGame.name;

                    var price = PriceHandler.ConvertPriceToDatabaseType(responseGame.default_sku.display_price, 2);

                    var game = new Game(title);

                    if (responseGame.images.Any())
                    {
                        game.Image = responseGame.images[0].url;
                    }

#if DEBUG
                    game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(title, TRAILER));
#endif

                    var gamePrices = new GamePrices(game.GameId, StoresEnum.Playstation.ToString(), price);

                    var history = new History(game.GameId, StoresEnum.Playstation.ToString(), gamePrices.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                    var genre = new Genre("Action");

                    entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
                }
            }

            return entities;
        }

        private string FormatName(string searchString) => searchString.Replace(" ", "+");
    }
}
