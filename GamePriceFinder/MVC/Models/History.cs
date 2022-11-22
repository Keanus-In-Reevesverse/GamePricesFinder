using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.MVC.Models
{
    /// <summary>
    /// Represents the historic of a game price, with history id, game id, store name, the price, and a timestamp indicating when that price was found.
    /// </summary>
    public class History
    {
        public History()
        {

        }

        public History(int gameId, int storeId, decimal price)
        {
            GameIdentifier = gameId;
            StoreIdentifier = storeId;
            Price = price;
            ChangeDate = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString();
        }

        [Column("game_id")]
        public int GameIdentifier { get; set; }
        [Column("store_id")]
        public int StoreIdentifier { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("change_date")]
        public string ChangeDate { get; set; }
    }
}
