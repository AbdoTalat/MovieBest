using MovieBest.DAL.Models;
using MovieBest.DAL.Entities;

namespace MovieBest.BLL.Services.Interfaces
{
    public interface IMovieService
    {
        public Task<IEnumerable<Movie>> GetAllMoviesAsync();
        public Task<Movie?> GetMovieByIdAsync(int Id);
        public Task<bool> AddNewMovieAsync(MovieViewModel movieVM, string uploadsImage, string uploadsVideo);
        public Task<bool> UpdateMovieAsync(MovieViewModel movieVM, int Id, string uploadsImage, string uploadsVideo);
        public Task<bool> DeleteMovieAsync(int Id, string uploadsImage);
    }
}
