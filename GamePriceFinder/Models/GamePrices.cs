using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.Models
{
    /// <summary>
    /// Represents a game price, with a game id, store id and price.
    /// </summary>


    [Table("game_prices")]
    public class GamePrices
    {
        public GamePrices(int gameId, string storeId, decimal currentPrice)
        {
            GameId = gameId;
            StoreId = storeId;
            CurrentPrice = currentPrice;
        }

        [Column("game_ID")]
        public int GameId { get; set; }
        [Column("store_name")]
        public string StoreId { get; set; }
        [Column("current_price")]
        public decimal CurrentPrice { get; set; }
    }
}
