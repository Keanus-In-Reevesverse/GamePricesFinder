using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.MVC.Models
{
    [Table("game_prices")]
    public class GamePrices
    {
        public GamePrices(int gameId, int storeId, decimal currentPrice, string link)
        {
            GameId = gameId;
            StoreId = storeId;
            CurrentPrice = currentPrice;
            Link = link;
        }

        [Column("game_ID")]
        public int GameId { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }
        [Column("current_price")]
        public decimal CurrentPrice { get; set; }
        [Column("link")]
        public string Link { get; set; }
    }
}
