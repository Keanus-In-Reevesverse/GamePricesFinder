using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Google.Apis.YouTube.v3;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Xbox price finder, implements IPriceFinder.
    /// </summary>
    public class EpicFinder : IPriceFinder
    {
        public EpicFinder()
        {
            HttpHandler = new HttpHandler();
        }
        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Epic.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

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
