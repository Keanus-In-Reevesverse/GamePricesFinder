using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Finders
{
    public class PlaystationStoreFinder : IPriceFinder
    {
        public PlaystationStoreFinder()
        {
            HttpHandler = new HttpHandler();
        }

        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var responseGameList = await HttpHandler.GetToPsn(FormatName(gameName));

            var entities = new List<DatabaseEntitiesHandler>();

            foreach (var responseGame in responseGameList)
            {
                var title = responseGame.name;

                var price = PriceHandler.ConvertPriceToDatabaseType(responseGame.default_sku.display_price, 2);

                var game = new Game(title, StoresEnum.PlaystationStore);

                var gamePrices = new GamePrices(game.GameId, ((int)StoresEnum.PlaystationStore).ToString(), price);

                var history = new History(game.GameId, StoresEnum.PlaystationStore.ToString(), gamePrices.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history));
            }

            return entities;
        }

        private string FormatName(string searchString) => searchString.Replace(" ", "+");
    }
}
