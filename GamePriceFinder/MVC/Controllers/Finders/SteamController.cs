using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;
using GamePriceFinder.MVC.Models.Responses;
using Genre = GamePriceFinder.MVC.Models.Genre;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    public class SteamController : IPriceFinder
    {
        private const int forHonorSteamId = 292030;

        private const string TRAILER = " trailer";

        public SteamController()
        {
            HttpHandler = new HttpController();
        }

        public string StoreUri { get; set; }
        public HttpController HttpHandler { get; set; }
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName, int id)
        {
            Dictionary<string, AppIds> steamResponse;
            try
            {
                steamResponse = await HttpHandler.GetToSteam(id);
            }
            catch (Exception e)
            {
                return null;
            }

            var name = string.Empty;

            var price = string.Empty;

            var entities = new List<DatabaseEntitiesHandler>();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                name = steamResponse[id.ToString()].data.name;

                price = steamResponse[id.ToString()].data.price_overview.final_formatted;

                var game = new Game(name);

                if (steamResponse[id.ToString()].data.screenshots.Any())
                {
                    game.Image = steamResponse[id.ToString()].data.screenshots[0].path_full;
                }

                //game.Video = steamResponse[forHonorSteamId.ToString()].data.movies[0].webm.max;
#if !DEBUG
                game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

                //await FillGameInformation(ref game, price, 3);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(price.Replace(".", ","), 3);

                var gamePrices = new GamePrices(game.GameId, StoresEnum.Steam.ToString(), currentPrice);

                var history = new History(game.GameId, StoresEnum.Steam.ToString(), currentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                var genre = new Genre("Action");

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
            }

            return entities;
        }
    }
}
