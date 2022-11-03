using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;
using GamePriceFinder.MVC.Models.Responses;
using Genre = GamePriceFinder.MVC.Models.Genre;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    public class PlaystationController : IPriceFinder
    {
        public PlaystationController()
        {
            HttpHandler = new HttpController();
        }

        private string GetLinkFromUrl(string url)
        {
            var urlInformation = url.Split("/");

            foreach (var part in urlInformation)
            {
                if (part.StartsWith("UP", StringComparison.CurrentCultureIgnoreCase))
                {
                    return part;
                }
            }

            return string.Empty;
        }

        public string StoreUri { get; set; }
        public HttpController HttpHandler { get; set; }
        private const string TRAILER = " trailer";
        public async Task<List<EntitiesHandler>> GetPrice(string gameName)
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

            var entities = new List<EntitiesHandler>();

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
                    var link = string.Concat("store.playstation.com/pt-br/product/", GetLinkFromUrl(responseGame.url));

                    var title = responseGame.name;

                    var price = PriceHandler.ConvertPriceToDatabaseType(responseGame.default_sku.display_price, 2);

                    var game = new Game(title);

                    if (responseGame.images.Any())
                    {
                        game.Image = responseGame.images[0].url;
                    }

#if !DEBUG
                    game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(title, TRAILER));
#endif

                    var gamePrices = new GamePrices(game.GameId, (int)StoresEnum.Playstation, price, link);

                    var history = new History(game.GameId, (int)StoresEnum.Playstation, gamePrices.CurrentPrice);

                    var genreStr = string.Empty;

                    try
                    {
                        if (responseGame.metadata.game_genre.values.Any())
                        {
                            genreStr = responseGame.metadata.game_genre.values[0].ToLower();

                            genreStr = genreStr.Replace("fighting", "Action");
                        }
                    }
                    catch (NullReferenceException)
                    {
                        genreStr = "Action";
                    }

                    var genre = new Genre(genreStr);

                    entities.Add(new EntitiesHandler(game, gamePrices, history, genre));
                }
            }

            return entities;
        }

        private string FormatName(string searchString) => searchString.Replace(" ", "+");
    }
}
