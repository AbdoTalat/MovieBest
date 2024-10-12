using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBest.DAL.Entities;
using MovieBest.DAL.Models;

namespace MovieBest.MVC.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly ILogger<AdminController> _logger;

		public AdminController(UserManager<ApplicationUser> userManager, IMapper mapper,
			RoleManager<ApplicationRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
			_roleManager = roleManager;
			_logger = logger;
		}

        #region Users
        [HttpGet]
		public async Task<IActionResult> Users()
		{
			var users = _userManager.Users.ToList();

			var mappedUser = users.Select(user => _mapper.Map<UserViewModel>(user)).ToList();

			foreach (var user in mappedUser)
			{
				var applicationUser = await _userManager.FindByNameAsync(user.UserName);
				if (applicationUser != null)
				{
					var roles = await _userManager.GetRolesAsync(applicationUser);
					user.UserRoles = roles.ToList();
				}
            }

			return View(mappedUser);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUser(string Id)
		{
			var user = await _userManager.FindByIdAsync(Id);
			if(user == null)
				return NotFound();

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteUserConfirm(string Id)
		{
			var user = await _userManager.FindByIdAsync(Id);
			if (user == null)
				return NotFound();
			try
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("Users");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return View(user);
			}
			return View(user);
		}

		[HttpGet]
		public async Task<IActionResult> EditUserRole(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return NotFound();
			var model = new UserRolesViewModel
			{
				UserID = userId,
				UserName = user.UserName,
				AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
				SelectedRoles = await _userManager.GetRolesAsync(user)
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUserRole(UserRolesViewModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserID);
			if (user == null)
				return NotFound();
			
			var currentRoles = await _userManager.GetRolesAsync(user);
			try
			{
				if (model.SelectedRoles == null || !model.SelectedRoles.Any())
				{
					var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
					if (removeResult.Succeeded)
						return RedirectToAction("Users");

					return View(model);
				}
				var result = await _userManager.RemoveFromRolesAsync(user, currentRoles.Except(model.SelectedRoles));

				result = await _userManager.AddToRolesAsync(user, model.SelectedRoles.Except(currentRoles));

				if (result.Succeeded)
					return RedirectToAction("Users");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return View(model);
			}

			return View(model);
		}
        #endregion


        #region Roles

        [HttpGet]
		public IActionResult GetRoles()	
			=> View(_roleManager.Roles.ToList());
		
		[HttpGet]
		public IActionResult AddNewRole()
			=> View();
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddNewRole(ApplicationRole role)
		{
			if (!ModelState.IsValid)
			{
				return View(role);
			}
			try
			{
				if (await _roleManager.RoleExistsAsync(role.Name) == true)
				{
					ModelState.AddModelError("", "This Role Already Exist.");
					return View(role);
				}
				ApplicationRole newRole = new ApplicationRole();
				newRole.Name = role.Name;

				var result = await _roleManager.CreateAsync(newRole);
				if (result.Succeeded)
				{
					return RedirectToAction("GetRoles");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				ModelState.AddModelError("", "An unexpected error occured. Please try Agian later.");
			}
			return View(role);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteRole(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);
			if (role == null)
				return NotFound();

			return View(role);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteRoleConfirm(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);
			if (role == null)
				return NotFound();

			try
			{
				var result = await _roleManager.DeleteAsync(role);
				if (result.Succeeded)
					return RedirectToAction("GetRoles");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return View(role);
		}

		[HttpGet]
		public async Task<IActionResult> GetUsersInRole(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);
			if (role == null)
				return NotFound();

			var users = await _userManager.GetUsersInRoleAsync(role.Name ?? "");
			return View(users);
		}

        #endregion
    }
}
