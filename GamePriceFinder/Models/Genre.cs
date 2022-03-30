using GamePriceFinder.Intefaces;

namespace GamePriceFinder.Models
{
    public class Genre : IGenre
    {
        public int GenreId { get; set; }
        public string Description { get; set; }
    }
}
