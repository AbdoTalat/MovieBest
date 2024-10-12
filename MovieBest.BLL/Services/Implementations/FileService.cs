using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieBest.BLL.Services.Interfaces;

namespace MovieBest.BLL.Services.Implementations
{
	public class FileService : IFileService
	{
		private readonly ILogger<FileService> _logger;

		public FileService(ILogger<FileService> logger)
		{
			_logger = logger;
		}

		public async Task<string?> SaveFileAsync(IFormFile file, string uploadsFolder, string folderName)
		{
			if (file == null || file.Length == 0) return null;

			string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
			string filePath = Path.Combine(uploadsFolder, fileName);

			try
			{
				if (!Directory.Exists(uploadsFolder))
					Directory.CreateDirectory(uploadsFolder);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				return $"/{folderName}/{fileName}";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error saving file.");
				return null;
			}
		}

		public async Task<bool> DeleteFileAsync(string? filePath, string? uploadsFolder)
		{
			if(filePath == null) 
				return false;

			string fullPath = Path.Combine(uploadsFolder, filePath.TrimStart('/'));
			

			if (!File.Exists(fullPath)) 
				return false;

			try
			{
				await Task.Run(() => File.Delete(fullPath));
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, "Error deleting file.");
				return false;
			}
		}

		public Task<string?> SaveImageAsync(IFormFile imageFile, string uploadsFolder) =>
			SaveFileAsync(imageFile, uploadsFolder, "Images");

		public Task<string?> SaveVideoAsync(IFormFile videoFile, string uploadsFolder) =>
			SaveFileAsync(videoFile, uploadsFolder, "Videos");
	}
}
