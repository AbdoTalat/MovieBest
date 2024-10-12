using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MovieBest.DAL.Models
{
    public class MovieViewModel
    {
        public int ID { get; set; }

		[Required(ErrorMessage = "Title Is Required.")]
        [MinLength(3)]
		public string Title { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "yyyy-mm-dd", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(10,1000)]
        public int Duration { get; set; }

        [Required]
        [Range(0,10)]
        public decimal Rating { get; set; }

        [Required]
        public string MovieLanguage { get; set; }

		public IFormFile? Image { get; set; } = null;
		public string? ExistingImageUrl { get; set; }

		public IFormFile? Video { get; set; } = null;
		public string? VideoUrl { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}
