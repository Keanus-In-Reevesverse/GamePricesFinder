﻿using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Finders
{
    /// <summary>
    /// Represents the Xbox price finder, implements IPriceFinder.
    /// </summary>
    public class EpicFinder : IPriceFinder
    {
        public EpicFinder()
        {
            HttpHandler = new HttpHandler();
        }
        /// <summary>
        /// Uri to execute the http request.
        /// </summary>
        public string StoreUri { get; set; }
        /// <summary>
        /// HttpHandler for Epic.
        /// </summary>
        public HttpHandler HttpHandler { get; set; }

        /// <summary>
        /// Gets Epic Games prices.
        /// </summary>
        /// <param name="gameName"></param>
        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var epicResponse = await HttpHandler.PostToEpic(gameName);

            var entities = new List<DatabaseEntitiesHandler>();

            for (int i = 0; i < epicResponse.Data.Catalog.SearchStore.Elements.Length; i++)
            {
                var currentGame = epicResponse.Data.Catalog.SearchStore.Elements[i];

                var title = currentGame.Title;

                var game = new Game(title);

                //await FillGameInformation(ref game, currentGame.Price.TotalPrice.FmtPrice.DiscountPrice, 2);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(currentGame.Price.TotalPrice.FmtPrice.DiscountPrice.Replace(".", ","), 2);

                var gamePrices = new GamePrices(
                    game.GameId,
                    ((int)StoresEnum.Epic).ToString(),
                    currentPrice);
                var history = new History(game.GenreId, StoresEnum.Epic.ToString(), currentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                var genre = new Genre("Action");

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
            }


            return entities;
        }
    }
}
