using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class GameData
    {
        public string VideoUrl { get; set; }
        public decimal CurrentPrice { get; set; } = 0;
        public string Description { get; set; }
    }
}
