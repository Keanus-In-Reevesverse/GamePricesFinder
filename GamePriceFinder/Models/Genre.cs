using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class GameGenre : IGenre
    {
        public int GenreId { get; set; }
        public string Description { get; set; }
    }
}
