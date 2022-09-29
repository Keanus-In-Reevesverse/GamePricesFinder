using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    /// <summary>
    /// Represents the Xbox price finder, implements IPriceFinder.
    /// </summary>
    public class EpicController : IPriceFinder
    {
        public EpicController()
        {
            HttpHandler = new HttpController();
        }
        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Epic.
        /// </summary>
        public HttpController HttpHandler { get; set; }

        private const string TRAILER = " trailer";

        /// <summary>
        /// Gets Epic Games prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            EpicGamesStoreNET.Models.Response epicResponse;
            try
            {
                epicResponse = await HttpHandler.PostToEpic(gameName);
            }
            catch (Exception e)
            {
                return null;
            }

            var entities = new List<DatabaseEntitiesHandler>();

            for (int i = 0; i < epicResponse.Data.Catalog.SearchStore.Elements.Length; i++)
            {
                var currentGame = epicResponse.Data.Catalog.SearchStore.Elements[i];

                var title = currentGame.Title;

                var game = new Game(title);

                if (currentGame.KeyImages.Any())
                {
                    game.Image = currentGame.KeyImages[0].Url;
                }

#if DEBUG
                game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(title, TRAILER));
#endif
                //await FillGameInformation(ref game, currentGame.Price.TotalPrice.FmtPrice.DiscountPrice, 2);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(currentGame.Price.TotalPrice.FmtPrice.DiscountPrice.Replace(".", ","), 2);

                var gamePrices = new GamePrices(game.GameId, StoresEnum.Epic.ToString(), currentPrice);

                var history = new History(game.GenreId, StoresEnum.Epic.ToString(), currentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                var genre = new Genre("Action");

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
            }


            return entities;
        }
    }
}
