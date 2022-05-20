using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class History
    {
        public History(int gameId, string storeName, decimal price, string changeDate)
        {
            GameId = gameId;
            StoreName = storeName;
            Price = price;
            ChangeDate = changeDate;
        }

        public int GameId { get; set; }
        public string StoreName { get; set; }
        public decimal Price { get; set; }
        public string ChangeDate { get; set; }
    }
}
