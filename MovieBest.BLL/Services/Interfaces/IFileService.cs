using Microsoft.AspNetCore.Http;

namespace MovieBest.BLL.Services.Interfaces
{
    public interface IFileService
    {
		//      public Task<string?> SaveImageAsync(IFormFile Image, string uploadsfolder);
		//      public Task<bool> DeleteImageAsync(string ImagePath, string uploadsfolder);
		//      public Task<string?> SaveVideoAsync(IFormFile Video, string uploadsfolder);
		//public Task<bool> DeleteVideoAsync(string VideoPath, string uploadsfolder);



		public Task<string?> SaveFileAsync(IFormFile file, string uploadsFolder, string folderName);
		public Task<bool> DeleteFileAsync(string filePath, string uploadsFolder);

		public Task<string?> SaveImageAsync(IFormFile imageFile, string uploadsFolder);
		public Task<string?> SaveVideoAsync(IFormFile videoFile, string uploadsFolder);
		//public Task<bool> DeleteImageAsync(string imagePath, string uploadsFolder);
		//public Task<bool> DeleteVideoAsync(string videoPath, string uploadsFolder);
	}
}
