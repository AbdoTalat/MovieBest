using Microsoft.Extensions.Logging;
using MovieBest.BLL.Services.Interfaces;
using MovieBest.BLL.UnitOfWork;
using MovieBest.DAL.Entities;

namespace MovieBest.BLL.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IUnitOfWork unitOfWork, ILogger<GenreService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        => await _unitOfWork.Repository<Genre>().GetAllAsync();
        

        public async Task<Genre?> GetGenreByIdAsync(int Id)
            => await _unitOfWork.Repository<Genre>().GetByIdAsync(Id);

        public async Task<bool> AddNewGenreAsync(Genre genre)
        {
            try
            {
                await _unitOfWork.Repository<Genre>().AddNewAsync(genre);
                return true;
            }
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
            }
        }

        public async Task<bool> updateGenreAsync(Genre genre)
        {
            try
            {
                await _unitOfWork.Repository<Genre>().UpdateAsync(genre);
                return true;
            }
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
            }
        }

        public async Task<bool> DeleteGenreAsync(int Id)
        {
            try
            {
                var isDeleted = await _unitOfWork.Repository<Genre>().DeleteAsync(Id);
                if (!isDeleted)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
