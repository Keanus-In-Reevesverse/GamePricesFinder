﻿using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;
using Google.Apis.YouTube.v3;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Playstation price finder, implements IPriceFinder.
    /// </summary>
    public class PlaystationStoreFinder : IPriceFinder
    {
        public PlaystationStoreFinder()
        {
            HttpHandler = new HttpHandler();
        }

        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Playstation Store.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

        private const string TRAILER = " trailer";

        /// <summary>
        /// Gets Playstation prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var responseGameList = await HttpHandler.GetToPsn(FormatName(gameName));

            var entities = new List<DatabaseEntitiesHandler>();

            foreach (var responseGame in responseGameList)
            {
                if (responseGame.default_sku == null)
                {
                    continue;
                }

                if (responseGame.default_sku.name.Equals("Jogo Completo", StringComparison.CurrentCultureIgnoreCase) ||
                    responseGame.default_sku.name.Equals("Jogo", StringComparison.CurrentCultureIgnoreCase) ||
                    responseGame.default_sku.name.Equals("Jogo Completo e Conteúdo Complementar", StringComparison.CurrentCultureIgnoreCase))
                {
                    var title = responseGame.name;

                    var price = PriceHandler.ConvertPriceToDatabaseType(responseGame.default_sku.display_price, 2);

                    var game = new Game(title);

                    if (responseGame.images.Any())
                    {
                        game.Image = responseGame.images[0].url;
                    }

#if !DEBUG
                    game.Video = await YoutubeHandler.GetGameTrailer(string.Concat(title, TRAILER));
#endif

                    var gamePrices = new GamePrices(game.GameId, ((int)StoresEnum.Playstation).ToString(), price);

                    var history = new History(game.GameId, StoresEnum.Playstation.ToString(), gamePrices.CurrentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                    var genre = new Genre("Action");

                    entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
                }
            }

            return entities;
        }

        private string FormatName(string searchString) => searchString.Replace(" ", "+");
    }
}
