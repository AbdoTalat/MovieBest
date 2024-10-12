using System.ComponentModel.DataAnnotations;

namespace MovieBest.DAL.Entities
{
    public class Genre
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name Is Required.")]
        [MinLength(3, ErrorMessage = "Name Must Be  At Least 3 Letters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description Is Required.")]
        [MinLength(4, ErrorMessage = "Description Must Be  At Least 4 Letters")]
        public string Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
