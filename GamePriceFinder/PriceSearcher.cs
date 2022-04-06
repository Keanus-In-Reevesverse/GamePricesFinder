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
            GetNuuvemPrices();

            ChangeGameNames(forHonorEpic.Name, forHonorSteam);

            return new List<Game> { forHonorSteam, forHonorEpic };
        }

        const string BASE_URL = "https://www.nuuvem.com";

        const string SERCH_PATH = "/catalog/search/";


        private void GetNuuvemPrices()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URL);
            var response = httpClient.GetAsync(SERCH_PATH + "gta").Result;
            var html = response.Content.ReadAsStringAsync().Result;

            

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div");

            var games = new List<Game>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        var newDoc = new HtmlAgilityPack.HtmlDocument();
                        newDoc.LoadHtml(div.InnerHtml);
                        //var insideDivs = newDoc.DocumentNode.Descendants("h3");
                        var price = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-price--val']").InnerText.Trim();
                        var name = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']").InnerText.Trim();
                        var game = new Game(name);
                        game.Store = Store.Nuuvem;
                        game.GameData.CurrentPrice = Convert.ToDecimal(price.Remove(0, 2));
                        games.Add(game);
                        //foreach (var insideDiv in insideDivs)
                        //{
                        //    if (insideDiv.Attributes["class"].Value == "product-title double-line-name")
                        //    {
                        //        var a = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']");
                        //    }
                        //}
                        //values.Add(div.Attributes["value"].Value);
                    }
                }
                catch (Exception)
                {
                    //ignored
                }
            }

            //engine.Execute("const log = console.log; const fetch = require('node-fetch'); const nuuvem = require('nuuvem'); const uri = 'https://localhost:7265/sendGameList/'; const https = require('https'); const agent = new https.Agent({rejectUnauthorized: false});let gameList = [];gameList = nuuvem.checkPrice('for honor').then(function (gameDataArray) {let results = []; gameDataArray.forEach(function (e) {var game = { title: e.title, price: e.price, currency: e.currency };results.push(game);});var json_text = JSON.stringify(results, null, 2);var request = encodeURI(uri + json_text);fetch(request, { agent }).then(function (response) {return response.json();}).then(function (jsonResponse) {console.log(jsonResponse);});});");
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
