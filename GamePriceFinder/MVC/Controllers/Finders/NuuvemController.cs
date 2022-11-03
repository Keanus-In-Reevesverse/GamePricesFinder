using GamePriceFinder.Handlers;
using GamePriceFinder.MVC.Models;
using GamePriceFinder.MVC.Models.Enums;
using GamePriceFinder.MVC.Models.Intefaces;
using Google.Apis.YouTube.v3.Data;

namespace GamePriceFinder.MVC.Controllers.Finders
{
    public class NuuvemController : IPriceFinder
    {
        public NuuvemController()
        {
            HttpHandler = new HttpController();
        }
        public string StoreUri { get; set; }
        public HttpController HttpHandler { get; set; }
        
        public async Task<List<EntitiesHandler>> GetPrice(string gameName)
        {
            string? html;
            try
            {
                html = await HttpHandler.GetToNuuvem(gameName);
            }
            catch (Exception)
            {
                return null;
            }

            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            var divs = doc.DocumentNode.Descendants("div");

            var entities = new List<EntitiesHandler>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        var entity = await ExtractPrices(div);

                        if (entity == null)
                        {
                            throw new ArgumentNullException(nameof(entity));
                        }

                        entities.Add(entity);
                    }
                }
                catch
                {
                    //ignored
                }
            }

            return entities;
        }

        private async Task<EntitiesHandler> ExtractPrices(HtmlAgilityPack.HtmlNode div)
        {
            var newDoc = new HtmlAgilityPack.HtmlDocument();
            newDoc.LoadHtml(div.InnerHtml);
            var bundle = string.Empty;
            var dlc = string.Empty;
            var link = string.Empty;
            bundle = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-badge product-badge__package']")?.InnerHtml.Trim();
            dlc = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-badge product-badge__dlc']")?.InnerHtml.Trim();
            if (!string.IsNullOrEmpty(bundle) || !string.IsNullOrEmpty(dlc))
            {
                return null;
            }
            else
            {
                var genreStr = newDoc.DocumentNode.SelectSingleNode("//div[@class='product__available product__purchasable product-card product-card__cover product-btn-add-to-cart--container']")?.Attributes["data-track-product-genre"]?.Value;
                
                var linkNode = newDoc.DocumentNode.SelectSingleNode("//a[@class='product-card--wrapper']");

                if (linkNode != null)
                {
                    link = linkNode.Attributes["href"].Value;
                }

                var price = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-price--val']").InnerText.Trim();
                var name = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']").InnerText.Trim();
                var game = new Game(name);

                var divWithImage = newDoc.DocumentNode.SelectSingleNode("//div[@class='product-img']")?.InnerHtml.Trim();

                var imageDoc = new HtmlAgilityPack.HtmlDocument();

                if (imageDoc != null)
                {
                    imageDoc.LoadHtml(divWithImage);
                    game.Image = imageDoc.DocumentNode.SelectSingleNode("//img")?.Attributes["src"]?.Value;
                }

#if !DEBUG
                game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

                var gamePrice = new GamePrices(game.GameId, (int)StoresEnum.Nuuvem, PriceHandler.ConvertPriceToDatabaseType(price, 3), link);
                var history = new History(game.GameId, (int)StoresEnum.Nuuvem, gamePrice.CurrentPrice);
                var genre = new Genre(genreStr == null ? String.Empty : genreStr);
                return new EntitiesHandler(game, gamePrice, history, genre);
            }
        }
    }
}
