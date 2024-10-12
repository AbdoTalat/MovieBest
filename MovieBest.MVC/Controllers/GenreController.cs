using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBest.BLL.Services.Interfaces;
using MovieBest.DAL.Entities;

namespace MovieBest.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var genres = await _genreService.GetAllGenresAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
				genres = genres.Where(m => m.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
			}
            return View(genres);
        }    

        [HttpGet]
        public IActionResult AddNew()
            => View();
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreService.AddNewGenreAsync(genre);
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var genre = await _genreService.GetGenreByIdAsync(Id);
            if (genre == null)
                return NotFound();

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreService.updateGenreAsync(genre);
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var genre = await _genreService.GetGenreByIdAsync(Id);
            if (genre == null)
                return NotFound();

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {
            if (!await _genreService.DeleteGenreAsync(Id))
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
