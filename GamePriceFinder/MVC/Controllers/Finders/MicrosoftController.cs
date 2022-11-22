using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;
using System.Net;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    public class MicrosoftController : IPriceFinder
    {
        public string StoreUri { get; set; }
        public HttpController HttpHandler { get; set; }
        private const string TRAILER = " trailer";

        private const string XboxDealsUrl = "https://xbdeals.net/br-store/search?search_query=";
        public async Task<List<EntitiesHandler>> GetPrice(string gameName)
        {
            var entities = new List<EntitiesHandler>();
            try
            {
                using var webClient = new WebClient();
                var urlToDownload = string.Concat(XboxDealsUrl, gameName);
                string html = webClient.DownloadString(new Uri(urlToDownload, new UriKind()));
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var divs = doc.DocumentNode.Descendants("div");

                var aS = doc.DocumentNode.Descendants("a");

                var image = string.Empty;
                var filled = false;
                Game game = null;
                GamePrices gamePrices = null;
                History history = null;
                Genre genre = null;

                foreach (var div in divs)
                {
                    try
                    {
                        var c = div.Attributes["class"];

                        if (c == null)
                        {
                            throw new Exception();
                        }

                        if (c.Value.Equals("col-md-2 col-sm-4 col-xs-6 game-collection-item-col", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var newDoc = new HtmlAgilityPack.HtmlDocument();
                            newDoc.LoadHtml(div.InnerHtml);

                            var insideDivs = newDoc.DocumentNode.Descendants("div");

                            var name = string.Empty;
                            var price = string.Empty;

                            foreach (var insideDiv in insideDivs)
                            {
                                name =
                                    newDoc.DocumentNode.SelectSingleNode("//p[@class='game-collection-item-details-title']")?.InnerText?.Trim();

                                price =
                                    newDoc.DocumentNode.SelectSingleNode("//span[@class='game-collection-item-regular-price ']")?.InnerText?.Trim();

                                if (string.IsNullOrEmpty(price))
                                {
                                    price = newDoc.DocumentNode.SelectSingleNode("//span[@class='game-collection-item-discount-price']")?.InnerText?.Trim();
                                }

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(price))
                                {
                                    break;
                                }
                            }

                            var convertedPrice = price.Remove(0, 3);

                            game = new Game(name);

                            game.Image = image;
                            image = string.Empty;

#if DEBUG
                            game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

                            gamePrices = new GamePrices(game.GameId, (int)StoresEnum.Xbox,
                                PriceHandler.ConvertPriceToDatabaseType(price, 3), "");

                            if (gamePrices.CurrentPrice == 0)
                            {
                                continue;
                            }

                            history = new History(
                                game.GameId, (int)StoresEnum.Xbox, gamePrices.CurrentPrice);
                            genre = new Genre("Action");

                            entities.Add(new EntitiesHandler(game, gamePrices, history, genre));
                        }

                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }

                var imagesToGet = entities.Count;
                var imagesFound = 0;
                var imageIndex = 0;
                foreach (var div in divs)
                {
                    try
                    {
                        if (imagesToGet == imagesFound)
                        {
                            break;
                        }

                        var c = div.Attributes["class"];

                        if (c == null)
                        {
                            throw new Exception();
                        }

                        if (c.Value.Equals("game-collection-item", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var newDoc = new HtmlAgilityPack.HtmlDocument();

                            newDoc.LoadHtml(div.InnerHtml);

                            var insideDivs = newDoc.DocumentNode.Descendants("div");


                            foreach (var insideDiv in insideDivs)
                            {
                                if (insideDiv.Attributes["class"].Value.Equals("game-collection-item-image-placeholder", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var imageDoc = new HtmlAgilityPack.HtmlDocument();

                                    imageDoc.LoadHtml(insideDiv.InnerHtml);

                                    var pictures = imageDoc.DocumentNode.Descendants("picture").ToList();

                                    if (pictures.Any())
                                    {
                                        imageDoc.LoadHtml(pictures[0].InnerHtml);

                                        image = imageDoc.DocumentNode.SelectSingleNode("//img")?.Attributes["data-src"]?.Value;

                                        imagesFound++;

                                        entities[imageIndex].Game.Image = image;

                                        imageIndex++;
                                    }

                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }

                var urlsToGet = entities.Count;
                var urlsFound = 0;
                var urlIndex = 0;

                foreach (var a in aS)
                {
                    try
                    {
                        if (urlsFound == urlsToGet)
                        {
                            break;
                        }

                        var c = a.Attributes["class"];

                        if (c == null)
                        {
                            throw new Exception();
                        }

                        if (c.Value.Equals("game-collection-item-link", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var link = string.Empty;

                            if (!string.IsNullOrEmpty(a.Attributes["href"].Value))
                            {
                                link = string.Concat("xbdeals.net", a.Attributes["href"].Value);
                            }

                            urlsFound++;

                            entities[urlIndex].GamePrices.Link = link;

                            urlIndex++;
                        }

                    }
                    catch (Exception e)
                    {
                        continue;
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
