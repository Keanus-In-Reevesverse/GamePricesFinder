using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using System.Net;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Xbox price finder, implements IPriceFinder.
    /// </summary>
    public class MicrosoftFinder : IPriceFinder
    {
        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Microsoft.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

        /// <summary>
        /// Gets Xbox prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var entities = new List<DatabaseEntitiesHandler>();
            try
            {
                using var webClient = new WebClient();
                var urlToDownload = string.Concat("https://xbdeals.net/br-store/search?search_query=", gameName);
                var kind = new UriKind();
                string html = webClient.DownloadString(new Uri(urlToDownload, kind));
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var divs = doc.DocumentNode.Descendants("div");

                foreach (var div in divs)
                {
                    try
                    {
                        if (div.Attributes["class"].Value == "game-collection-item-details")
                        {
                            var newDoc = new HtmlAgilityPack.HtmlDocument();
                            newDoc.LoadHtml(div.InnerHtml);
                            var encodedName = newDoc.DocumentNode.SelectSingleNode("//p[@class='game-collection-item-details-title']").InnerText.Trim();
                            var name = DecodeWithBruteForce(ref encodedName);

                            var newDoc2 = new HtmlAgilityPack.HtmlDocument();
                            var spans = newDoc.DocumentNode.Descendants("span").ToList();

                            var price = string.Empty;

                            foreach (var span in spans)
                            {
                                try
                                {
                                    if (span.Attributes["itemprop"].Value == "price")
                                    {
                                        price = newDoc.DocumentNode.SelectSingleNode("//span[@itemprop='price']").InnerText.Trim();
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //ignored
                                }

                            }
                            
                            var game = new Game(name);

                            var gamePrices = new GamePrices(
                                game.GameId, ((int)StoresEnum.Xbox).ToString(), 
                                PriceHandler.ConvertPriceToDatabaseType(price, 3));

                            if (gamePrices.CurrentPrice == 0)
                            {
                                continue;
                            }

                            var history = new History(
                                game.GameId, StoresEnum.Xbox.ToString(), gamePrices.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
                            var genre = new Genre("Action");

                            entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
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
            encodedString = encodedString.Replace("&#225;", "á");
            encodedString = encodedString.Replace("&amp", string.Empty);
            return encodedString;
        }
    }
}
