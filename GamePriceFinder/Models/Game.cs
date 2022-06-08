using GamePriceFinder.Enums;
using GamePriceFinder.Intefaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamePriceFinder.Models
{
    /// <summary>
    /// Represents a game, with game id, genre id, promotional video url, image path, a short description, the publisher name and the name of the game.
    /// </summary>
    [Table("game")]
    public class Game
    {
        private string _image;

        public Game(string name)
        {
            Name = name;
        }

        [Column("genre_id")]
        public int GenreId { get; set; }
        [Column("game_id")]
        public int GameId { get; set; }
        [Column("video")]
        public string Video { get; set; }

        [Column("game_image")]
        public string Image{ get; set; }


        [Column("description")]
        public string Description { get; set; }
        [Column("publisher")]
        public string Publisher { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }

}
