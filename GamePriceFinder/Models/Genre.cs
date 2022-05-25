using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.Models
{
    public class Genre
    {
        public Genre(string description)
        {
            Description = description;
        }

        [Column("genre_id")]
        public int GenreId { get; set; }
        public string Description { get; set; }
    }
}
