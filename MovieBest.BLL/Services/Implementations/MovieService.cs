using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieBest.DAL.Models;
using MovieBest.BLL.Services.Interfaces;
using MovieBest.BLL.UnitOfWork;
using MovieBest.DAL.Entities;

namespace MovieBest.MVC.Core.Services.Implementations
{
	public class MovieService : IMovieService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;
		private readonly ILogger<MovieService> _logger;

		public MovieService(IUnitOfWork unitOfWork, IFileService  fileService, IMapper mapper, ILogger<MovieService> logger)
		{
			_unitOfWork = unitOfWork;
			_fileService = fileService;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
			 => await _unitOfWork.Repository<Movie>().GetAllAsync();

		public async Task<Movie?> GetMovieByIdAsync(int Id)
			=> await _unitOfWork.Repository<Movie>().GetSingleWithIncludeAsync(m => m.ID == Id, query => query.Include(m => m.Genre));

		public async Task<bool> AddNewMovieAsync(MovieViewModel movieVM, string uploadsImage, string uploadsVideo)
		{
			try
			{
				string? imageUrl = await _fileService.SaveImageAsync(movieVM.Image, uploadsImage);
				string? videoUrl = await _fileService.SaveVideoAsync(movieVM.Video, uploadsVideo);

				var mappedMovie = _mapper.Map<Movie>(movieVM);
				mappedMovie.ImageUrl = imageUrl;
				mappedMovie.VideoUrl = videoUrl;

				await _unitOfWork.Repository<Movie>().AddNewAsync(mappedMovie);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding new movie.");
				return false;
			}
		}

		public async Task<bool> UpdateMovieAsync(MovieViewModel movieVM, int id, string uploadsImage, string uploadsVideo)
		{
			var oldMovie = await _unitOfWork.Repository<Movie>().GetByIdAsync(id);
			if (oldMovie == null) return false;
	
			string? imageUrl = oldMovie.ImageUrl;
			string? videoUrl = oldMovie.VideoUrl;

			try
			{
				if (movieVM.Image != null)
				{
					 imageUrl = await _fileService.SaveImageAsync(movieVM.Image, uploadsImage);
					if (!string.IsNullOrEmpty(oldMovie.ImageUrl))
						await _fileService.DeleteFileAsync(oldMovie.ImageUrl.Substring("/Images".Length), uploadsImage);
				}
				if (movieVM.Video != null)
				{
				    videoUrl = await _fileService.SaveVideoAsync(movieVM.Video, uploadsVideo);
					if (!string.IsNullOrEmpty(oldMovie.VideoUrl))
					{
						bool sssssss = await _fileService.DeleteFileAsync(oldMovie.VideoUrl.Substring("/Videos".Length), uploadsVideo);
					}
				}

				_mapper.Map(movieVM, oldMovie);
				oldMovie.ImageUrl = imageUrl;
				oldMovie.VideoUrl = videoUrl;

				await _unitOfWork.Repository<Movie>().UpdateAsync(oldMovie);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, "Error updating movie.");
				return false;
			}
		}

		public async Task<bool> DeleteMovieAsync(int Id, string uploadsImage)
		{
			var movie = await _unitOfWork.Repository<Movie>().GetByIdAsync(Id);
			if (movie == null)
				return false;

			try
			{
				await _fileService.DeleteFileAsync(movie.ImageUrl, uploadsImage);
				await _fileService.DeleteFileAsync(movie.VideoUrl, uploadsImage);
				bool isMovieDeleted = await _unitOfWork.Repository<Movie>().DeleteAsync(Id);
				if (isMovieDeleted == true)
					return true;

				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;
			}
		}
	}
}
