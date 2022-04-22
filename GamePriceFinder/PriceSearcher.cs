using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Models;

namespace GamePriceFinder
{
    //https://stackoverflow.com/questions/36975619/how-to-call-a-rest-web-service-api-from-javascript
    //https://stackoverflow.com/questions/69478024/require-not-supported-even-though-im-using-import
    //https://stackoverflow.com/questions/1973140/parsing-json-from-xmlhttprequest-responsejson
    //https://www.freecodecamp.org/news/here-is-the-most-popular-ways-to-make-an-http-request-in-javascript-954ce8c95aaa/#:~:text=To%20make%20an%20HTTP%20call,to%20fire%20off%20the%20request.
    

    public class PriceSearcher
    {
        private const int forHonorSteamId = 271590;

        private readonly ApiStoresHandler _apiStoresHandler;

        private readonly PriceHandler _priceHandler;

        const string BASE_URL = "https://www.nuuvem.com";

        const string SERCH_PATH = "/catalog/search/";

        public PriceSearcher()
        {
            _priceHandler = new PriceHandler();
            _apiStoresHandler = new ApiStoresHandler(_priceHandler);
        }

        public async Task<List<Game>> GetPrices(string gameName)
        {
            var forHonorSteam = await GetSteamPrice();

            var forHonorEpic = await GetEpicPrice(gameName);

            var nuuvemGames = await GetNuuvemPrices();

            return new List<Game> { new Game("isdfhg", Store.Steam), forHonorEpic };
        }

        private async Task<List<Game>> GetNuuvemPrices()
        {
            var html = await _apiStoresHandler.GetToNuuvem("gta");

            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div");

            var gameList = new List<Game>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        gameList.Add(ExtractPrices(div));
                    }
                }
                catch (Exception)
                {
                    //ignored
                }
            }

            return gameList;
        }

        private Game ExtractPrices(HtmlAgilityPack.HtmlNode div)
        {
            var newDoc = new HtmlAgilityPack.HtmlDocument();
            newDoc.LoadHtml(div.InnerHtml);
            var price = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-price--val']").InnerText.Trim();
            var name = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']").InnerText.Trim();
            var game = new Game(name, Store.Nuuvem);
            game.GameData.CurrentPrice = Convert.ToDecimal(price.Remove(0, 2));
            return game;
        }

        private void ChangeGameNames(string gameName, params Game[] games)
        {
            for (int i = 0; i < games.Length; i++)
            {
                games[i].Name = gameName;
            }
        }

        

        private async Task<List<Game>> GetSteamPrice()
        {
            var steamResponse = await _apiStoresHandler.GetToSteam(forHonorSteamId);

            var gameName = string.Empty;

            var price = string.Empty;

            var steamGames = new List<Game>();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                gameName = steamResponse[forHonorSteamId.ToString()].data.name;

                price = steamResponse[forHonorSteamId.ToString()].data.price_overview.final_formatted;

                var game = new Game(gameName, Store.Steam);
                
                steamGames.Add(game);

                await FillGameInformation(ref game, price, 3);

            }

            return steamGames;
        }

        private Task FillGameInformation(ref Game game, string formattedPrice, int cutPrice)
        {
            _priceHandler.PriceFormatted = formattedPrice.Replace(".", ",");

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

            var game = new Game(searchedGame.Title, Store.Epic);

            await FillGameInformation(ref game, searchedGame.Price.TotalPrice.FmtPrice.DiscountPrice, 2);

            return game;
        }
    }
}
