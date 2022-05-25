using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Finders
{
    public class NuuvemFinder : IPriceFinder
    {
        public NuuvemFinder()
        {
            HttpHandler = new HttpHandler();
        }
        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var html = await HttpHandler.GetToNuuvem(gameName);

            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div");

            var entities = new List<DatabaseEntitiesHandler>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        entities.Add(ExtractNuuvemPrices(div));
                    }
                }
                catch
                {
                    //ignored
                }
            }

            return entities;
        }

        private DatabaseEntitiesHandler ExtractNuuvemPrices(HtmlAgilityPack.HtmlNode div)
        {
            var newDoc = new HtmlAgilityPack.HtmlDocument();
            newDoc.LoadHtml(div.InnerHtml);
            var price = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-price--val']").InnerText.Trim();
            var name = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']").InnerText.Trim();
            var game = new Game(name);
            var gamePrice = new GamePrices(game.GameId, ((int)StoresEnum.Nuuvem).ToString(), PriceHandler.ConvertPriceToDatabaseType(price, 3));
            var history = new History(game.GameId, StoresEnum.Nuuvem.ToString(), gamePrice.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            var genre = new Genre("Action");
            return new DatabaseEntitiesHandler(game, gamePrice, history, genre);
        }
    }
}
