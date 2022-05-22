using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using PuppeteerSharp;
using System.Net;
using System.Text;
using System.Web;

namespace GamePriceFinder.Finders
{
    public class MicrosoftFinder : IPriceFinder
    {
        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var entities = new List<DatabaseEntitiesHandler>();
            try
            {
                using var webClient = new WebClient();
                var urlToDownload = string.Concat("https://www.microsoft.com/pt-br/search/shop?q=",gameName);
                string html = webClient.DownloadString(urlToDownload);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var divs = doc.DocumentNode.Descendants("div");

                foreach (var div in divs)
                {
                    try
                    {
                        if (div.Attributes["class"].Value == "c-channel-placement-content")
                        {
                            var newDoc = new HtmlAgilityPack.HtmlDocument();
                            newDoc.LoadHtml(div.InnerHtml);
                            var encodedName = newDoc.DocumentNode.SelectSingleNode("//h3[@class='c-subheading-6']").InnerText.Trim();
                            var name = DecodeWithBruteForce(ref encodedName);
                            var price = newDoc.DocumentNode.SelectSingleNode("//span[@itemprop='price']").InnerText.Trim();
                            var game = new Game(name, StoresEnum.XboxStore);
                            var gamePrices = new GamePrices(
                                game.GameId, ((int)StoresEnum.XboxStore).ToString(), 
                                PriceHandler.ConvertPriceToDatabaseType(price, 3));
                            var history = new History(
                                game.GameId, StoresEnum.XboxStore.ToString(), gamePrices.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
                            if (gamePrices.CurrentPrice == 0)
                            {
                                continue;
                            }
                            entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history));
                        }
                    }
                    catch (Exception e)
                    {
                        //ignored
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ", e.Message);
            }
            return entities;
        }

        private static string DecodeWithBruteForce(ref string encodedString)
        {
            encodedString = encodedString.Replace("&#243;", "ó");
            encodedString = encodedString.Replace("&#231;&#227;", "çã");
            encodedString = encodedString.Replace("&#227;", "ã");
            encodedString = encodedString.Replace("&#174;", "®");
            encodedString = encodedString.Replace("&amp", String.Empty);
            return encodedString;
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

    }
}
