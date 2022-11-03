using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.MVC.Models
{
    /// <summary>
    /// Represents the historic of a game price, with history id, game id, store name, the price, and a timestamp indicating when that price was found.
    /// </summary>
    public class History
    {
        public History(int gameId, int storeId, decimal price)
        {
            GameId = gameId;
            StoreId = storeId;
            Price = price;
            ChangeDate = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString();
        }

        [Key]
        [Column("change_id")]
        public int HistoryId { get; set; }

        [Column("game_id")]
        public int GameId { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }
        [Column("price")]

        public decimal Price { get; set; }
        [Column("change_date")]

        public string ChangeDate { get; set; }
    }
}
