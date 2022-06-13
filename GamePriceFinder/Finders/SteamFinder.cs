using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Steam price finder, implements IPriceFinder.
    /// </summary>
    public class SteamFinder : IPriceFinder
    {
        private const int forHonorSteamId = 292030;

        private const string TRAILER = " trailer";

        public SteamFinder()
        {
            HttpHandler = new HttpHandler();
        }

        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Playstation Store.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

        /// <summary>
        /// Gets Steam prices.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName, int id)
        {
            var steamResponse = await HttpHandler.GetToSteam(id);

            var name = string.Empty;

            var price = string.Empty;

            var entities = new List<DatabaseEntitiesHandler>();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                name = steamResponse[id.ToString()].data.name;

                price = steamResponse[id.ToString()].data.price_overview.final_formatted;

                var game = new Game(name);

                //game.Video = steamResponse[forHonorSteamId.ToString()].data.movies[0].webm.max;
#if !DEBUG
                game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

                //await FillGameInformation(ref game, price, 3);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(price.Replace(".", ","), 3);

                var gamePrices = new GamePrices(game.GameId, ((int)StoresEnum.Steam).ToString(), currentPrice);

                var history = new History(game.GameId, StoresEnum.Steam.ToString(), currentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                var genre = new Genre("Action");

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
            }

            return entities;
        }
    }
}
