﻿using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Google.Apis.YouTube.v3;
using System.Text.RegularExpressions;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Nuuvem price finder, implements IPriceFinder.
    /// </summary>
    public class NuuvemFinder : IPriceFinder
    {
        public NuuvemFinder()
        {
            HttpHandler = new HttpHandler();
        }
        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Nuuvem.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

        private const string TRAILER = " trailer";

        /// <summary>
        /// Gets Nuuvem prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
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

            var entities = new List<DatabaseEntitiesHandler>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        var entity = await ExtractNuuvemPrices(div);

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

        private async Task<DatabaseEntitiesHandler> ExtractNuuvemPrices(HtmlAgilityPack.HtmlNode div)
        {
            var newDoc = new HtmlAgilityPack.HtmlDocument();
            newDoc.LoadHtml(div.InnerHtml);
            var bundle = string.Empty;
            var dlc = string.Empty;

            bundle = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-badge product-badge__package']")?.InnerHtml.Trim();
            dlc = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-badge product-badge__dlc']")?.InnerHtml.Trim();
            if (!string.IsNullOrEmpty(bundle) || !string.IsNullOrEmpty(dlc))
            {
                return null;
            }
            else
            {

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

#if DEBUG
                game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(name, TRAILER));
#endif

                var gamePrice = new GamePrices(game.GameId, StoresEnum.Nuuvem.ToString(), PriceHandler.ConvertPriceToDatabaseType(price, 3));
                var history = new History(game.GameId, StoresEnum.Nuuvem.ToString(), gamePrice.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
                var genre = new Genre("Action");
                return new DatabaseEntitiesHandler(game, gamePrice, history, genre);
            }
        }
    }
}
