using GamePriceFinder.Intefaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.Models
{
    /// <summary>
    /// Represents the historic of a game price, with history id, game id, store name, the price, and a timestamp indicating when that price was found.
    /// </summary>
    public class History
    {
        public History(int gameId, string storeName, decimal price, string changeDate)
        {
            GameId = gameId;
            StoreName = storeName;
            Price = price;
            ChangeDate = changeDate;
        }

        [Key]
        [Column("change_id")]
        public int HistoryId { get; set; }

        [Column("game_id")]
        public int GameId { get; set; }
        [Column("store_name")]

        public string StoreName { get; set; }
        [Column("price")]

        public decimal Price { get; set; }
        [Column("change_date")]

        public string ChangeDate { get; set; }
    }
}
