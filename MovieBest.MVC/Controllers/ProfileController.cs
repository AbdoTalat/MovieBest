using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBest.DAL.Entities;
using MovieBest.DAL.Models;

namespace MovieBest.MVC.Controllers
{
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger<ProfileController> _logger;

		public ProfileController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
			,SignInManager<ApplicationUser> signInManager, ILogger<ProfileController> logger)
        {
			_userManager = userManager;
            _roleManager = roleManager;
			_signInManager = signInManager;
			_logger = logger;
		}

		[HttpGet]
        public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			
			if (user == null)
				return RedirectToAction("Login", "Account");

			var userRoles = await _userManager.GetRolesAsync(user);

			var model = new ProfileViewModel()
			{
				userName = user.UserName,
				Email = user.Email,
				userRoles = userRoles
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult ChangePassword()
			=> View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
					if (result.Succeeded)
					{
						await _signInManager.SignOutAsync();
						TempData["SuccessMessage"] = "Your Password Hase Been Changed Successfully";
						return RedirectToAction("Login", "Account");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return View(model);
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteProfile()
		{ 
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteProfileConfirm()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			try
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					TempData["DeleteProfile"] = "Profile Deleted Successfully";
					await _signInManager.SignOutAsync();
					return RedirectToAction("Index", "Home");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return View();
			}
			return View();
		}
	}
}
