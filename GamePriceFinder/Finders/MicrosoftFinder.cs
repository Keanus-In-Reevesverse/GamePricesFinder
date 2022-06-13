using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Google.Apis.YouTube.v3;
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

        private const string TRAILER = " trailer";

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
                        if (div.Attributes["class"].Value == "col-md-2 col-sm-4 col-xs-6 game-collection-item-col")
                        {
                            var newDoc = new HtmlAgilityPack.HtmlDocument();
                            newDoc.LoadHtml(div.InnerHtml);

                            var insideDivs = newDoc.DocumentNode.Descendants("div");

                            var name = string.Empty;
                            var price = string.Empty;

                            foreach (var insideDiv in insideDivs)
                            {
                                name = 
                                    newDoc.DocumentNode.SelectSingleNode("//p[@class='game-collection-item-details-title']").InnerText.Trim();
                                price =
                                    newDoc.DocumentNode.SelectSingleNode("//span[@class='game-collection-item-regular-price ']").InnerText.Trim();

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(price))
                                {
                                    break;
                                }
                            }

                            var convertedPrice = price.Remove(0, 3);

                            var game = new Game(name);
#if !DEBUG
                            game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

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
