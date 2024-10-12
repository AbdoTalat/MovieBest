using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieBest.BLL.Services.Interfaces;
using MovieBest.DAL.Entities;
using MovieBest.DAL.Models;
using MovieBest.MVC.Helpers;

namespace MovieBest.MVC.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public MovieController(IMovieService movieService, IGenreService genreService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _movieService = movieService;
            _genreService = genreService;
            this.mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var movies = await _movieService.GetAllMoviesAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            var count = movies.Count();
            var data = movies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var model = new Pagination<Movie>(pageNumber, pageSize, count, data);
            ViewBag.PageSizeList = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = "5", Text = "5" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "15", Text = "15" },
                }, "Value", "Text", pageSize);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var movie = await _movieService.GetMovieByIdAsync(Id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddNew()
        {
            ViewData["Genres"] = await _genreService.GetAllGenresAsync();

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew(MovieViewModel movieVM)
        {
            if (movieVM.GenreId == 0)
            {
                ModelState.AddModelError("GenreId", "You Need to Choose Movie Genre");
            }
            if (ModelState.IsValid)
            {
				
				bool check = await _movieService.AddNewMovieAsync(movieVM, FilePath("Images"), FilePath("Videos"));
                if (check)
                    return RedirectToAction("Index", "Movie");
            }

            ViewData["Genres"] = await _genreService.GetAllGenresAsync();
            return View(movieVM);
        }
        

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var movie = await _movieService.GetMovieByIdAsync(Id);
            if (movie == null)
                return NotFound();

            var mappedMovie = mapper.Map<MovieViewModel>(movie);

            mappedMovie.ExistingImageUrl = movie.ImageUrl;

            ViewData["Genres"] = await _genreService.GetAllGenresAsync();
            return View(mappedMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieViewModel movieVM, int Id)
        {
            if (movieVM.GenreId == 0)
            {
                ModelState.AddModelError("GenreId", "You Need to Choose Movie Genre");
            }

            if (ModelState.IsValid)
            {
                bool check = await _movieService.UpdateMovieAsync(movieVM, Id, FilePath("Images"), FilePath("Videos"));
                if (check)
                    return RedirectToAction("Index");
            }

            ViewData["Genres"] = await _genreService.GetAllGenresAsync();
            return View(movieVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var movie = await _movieService.GetMovieByIdAsync(Id);
            if (movie == null) 
                return NotFound();

            return View(movie);
        } 

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {
            try
            {
                bool isDeleted = await _movieService.DeleteMovieAsync(Id, FilePath(""));
                if (isDeleted)
                    return RedirectToAction("Index", "Movie");

                return View("Delete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> WatchMovie(int Id)
        {
            var movie = await _movieService.GetMovieByIdAsync(Id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

		private string FilePath(string? filePath)
        {
            if (filePath == null)
                return Path.Combine(_webHostEnvironment.WebRootPath);

            return Path.Combine(_webHostEnvironment.WebRootPath, filePath);
		}
			
	}
}




