﻿using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class GamePrices
    {
        public GamePrices(int gameId, string storeId, decimal currentPrice)
        {
            GameId = gameId;
            StoreId = storeId;
            CurrentPrice = currentPrice;
        }
        public int GameId { get; set; }
        public string StoreId { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}