﻿using GamePriceFinder.Enums;
using GamePriceFinder.Handlers;
using GamePriceFinder.Http;
using GamePriceFinder.Intefaces;
using GamePriceFinder.Models;

namespace GamePriceFinder.Finders
{
    public class SteamFinder : IPriceFinder
    {
        private const int forHonorSteamId = 304390;
        public SteamFinder()
        {
            HttpHandler = new HttpHandler();
        }

        public string StoreUri { get; set; }
        public HttpHandler HttpHandler { get; set; }

        public async Task<List<DatabaseEntitiesHandler>> GetPrice(string gameName)
        {
            var steamResponse = await HttpHandler.GetToSteam(forHonorSteamId);

            gameName = string.Empty;

            var price = string.Empty;

            var entities = new List<DatabaseEntitiesHandler>();

            for (int responseObject = 0; responseObject < steamResponse.Count; responseObject++)
            {
                gameName = steamResponse[forHonorSteamId.ToString()].data.name;

                price = steamResponse[forHonorSteamId.ToString()].data.price_overview.final_formatted;

                var game = new Game(gameName);

                //await FillGameInformation(ref game, price, 3);

                var currentPrice = PriceHandler.ConvertPriceToDatabaseType(price.Replace(".", ","), 3);

                var gamePrices = new GamePrices(game.GameId, ((int)StoresEnum.Steam).ToString(), currentPrice);

                var history = new History(game.GameId, StoresEnum.Steam.ToString(), currentPrice, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

                var genre = new Genre("Action");

                entities.Add(new DatabaseEntitiesHandler(game, gamePrices, history, genre));
            }

            return entities;
        }
    }
}
