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
        public async Task<List<EntitiesHandler>> GetPrice(string gameName, int id)
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

            var entities = new List<EntitiesHandler>();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                AppIds currentGame = steamResponse[id.ToString()];

                name = currentGame.data.name;

                price = currentGame.data.price_overview.final_formatted;

                var game = new Game(name);

                if (currentGame.data.screenshots.Any())
                {
                    game.Image = currentGame.data.screenshots[0].path_full;
                }

                var link = string.Concat("store.steampowered.com/app/", id);

                if (currentGame.data.movies.Any())
                {
                    foreach (var movie in currentGame.data.movies)
                    {
                        try
                        {
                            game.Video = movie.mp4.max;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(game.Video))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    //game.Video = steamResponse[forHonorSteamId.ToString()].data.movies[0].webm.max;
#if !DEBUG
                    game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif
                }


                //await FillGameInformation(ref game, price, 3);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(price.Replace(".", ","), 3);

                var gamePrices = new GamePrices(game.GameId, (int)StoresEnum.Steam, currentPrice, link);

                var history = new History(game.GameId, (int)StoresEnum.Steam, currentPrice);

                Genre genre = null;

                if (currentGame.data.genres.Any())
                {
                    genre = new Genre(currentGame.data.genres[0].description);
                }

                entities.Add(new EntitiesHandler(game, gamePrices, history, genre));
            }

            return entities;
        }
    }
}
