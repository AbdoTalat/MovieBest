using MovieBest.DAL.Entities;

namespace MovieBest.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        public Task<IEnumerable<Genre>> GetAllGenresAsync();
        public Task<Genre?> GetGenreByIdAsync(int Id);
        public Task<bool> AddNewGenreAsync(Genre genre);
        public Task<bool> updateGenreAsync(Genre genre);
        public Task<bool> DeleteGenreAsync(int Id);
    }
}
