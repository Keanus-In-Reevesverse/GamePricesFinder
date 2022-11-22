using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.MVC.Models
{
    /// <summary>
    /// Represents available game genres, with game ids and genre description.
    /// </summary>
    public class Genre
    {
        public Genre()
        {

        }
        public Genre(string description)
        {
            Description = description;
        }

        [Column("genre_id")]
        public int GenreId { get; set; }
        public string Description { get; set; }
    }
}
