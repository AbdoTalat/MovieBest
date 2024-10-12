using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBest.DAL.Entities
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public decimal Rating { get; set; }
        public string MovieLanguage { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public virtual Genre? Genre { get; set; }
    }
}
