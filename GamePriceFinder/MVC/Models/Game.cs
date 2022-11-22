using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.MVC.Models
{
    [Table("game")]
    public class Game
    {
        private string _image;

        public Game()
        {

        }
        public Game(string name)
        {
            Name = name;
        }

        [Column("genre_id")]
        public int GenreId { get; set; }
        [Column("game_id")]
        public int GameId { get; set; }
        [Column("video")]
        public string Video { get; set; } = string.Empty;

        [Column("game_image")]
        public string Image { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;
        [Column("publisher")]
        public string Publisher { get; set; } = string.Empty;
        [Column("name")]
        public string Name { get; set; }
    }

}
