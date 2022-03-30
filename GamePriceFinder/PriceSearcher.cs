using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Models;

namespace GamePriceFinder
{
    public class PriceSearcher
    {
        private readonly ApiStoresHandler _apiStoresHandler;
        private readonly PriceHandler _priceHandler;
        public PriceSearcher()
        {
            _priceHandler = new PriceHandler();
            _apiStoresHandler = new ApiStoresHandler(_priceHandler);
        }

        public async Task<List<Game>> GetPrices(string gameName)
        {
            var forHonorSteam = await GetSteamPrice();
            var forHonorEpic = await GetEpicPrice(gameName);

            ChangeGameNames(forHonorEpic.Name, forHonorSteam);

            return new List<Game> { forHonorSteam, forHonorEpic };
        }

        private void ChangeGameNames(string gameName, params Game[] games)
        {
            for (int i = 0; i < games.Length; i++)
            {
                games[i].Name = gameName;
            }
        }

        private const int forHonorSteamId = 304390;

        private async Task<Game> GetSteamPrice()
        {
            var steamResponse = await _apiStoresHandler.GetToSteam(forHonorSteamId);

            var gameName = string.Empty;

            var price = string.Empty;

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                gameName = steamResponse[forHonorSteamId.ToString()].data.name;

                price = steamResponse[forHonorSteamId.ToString()].data.price_overview.final_formatted;
            }

            var game = new Game(gameName);

            await FillGameInformation(ref game, Store.Steam, price, 3);

            return game;
        }

        private Task FillGameInformation(ref Game game, Store publisher, string formattedPrice, int cutPrice)
        {
            _priceHandler.PriceFormatted = formattedPrice.Replace(".", ",");

            game.Store = publisher;

            var currentPrice = _priceHandler.ConvertPriceToDatabaseType(cutPrice);

            game.GameData.CurrentPrice = currentPrice;

            game.History.Price = currentPrice;

            game.History.ChangeDate = DateTime.Now.ToShortDateString();

            return Task.CompletedTask;
        }

        private async Task<Game> GetEpicPrice(string gameName)
        {
            var epicResponse = await _apiStoresHandler.PostToEpic(gameName);

            var searchedGame = epicResponse.Data.Catalog.SearchStore.Elements[0];

            var game = new Game(searchedGame.Title);

            await FillGameInformation(ref game, Store.Epic, searchedGame.Price.TotalPrice.FmtPrice.DiscountPrice, 2);

            return game;
        }
    }
}
