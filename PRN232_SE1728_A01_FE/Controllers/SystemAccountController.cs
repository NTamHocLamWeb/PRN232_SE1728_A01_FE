using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using System.Security.Claims;
using PRN232_SE1728_A01_FE.Services.Enums;

namespace PRN232_SE1728_A01_FE.Controllers
{
	public class SystemAccountController : Controller
	{
		private readonly ISystemAccountService _systemAccountService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SystemAccountController(ISystemAccountService systemAccountService, IHttpContextAccessor httpContextAccessor)
		{
			_systemAccountService = systemAccountService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(SystemAccountDTO account)
		{
			try
			{
				var user = await _systemAccountService.GetSystemAccountAsync(account.AccountEmail, account.AccountPassword);
				if (user == null)
				{
					ModelState.AddModelError("AccountEmail", "Email or password is incorrect");
					return RedirectToAction("Login");
				}

				// Tạo ClaimsIdentity
				var claims = new List<Claim>
				{
					new(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
					new(ClaimTypes.Name, user.AccountName ?? ""),
					new(ClaimTypes.Email, user.AccountEmail ?? ""),
					new(ClaimTypes.Role, user.AccountRole.ToString()),
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
				};

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
				return RedirectToAction("Index", "NewsArticle");
			}
			catch
			{
				return RedirectToAction("Login");
			}
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "SystemAccount");
		}

		public async Task<IActionResult> Index()
		{
			var result = await _systemAccountService.GetListAccount();
			ViewBag.Role = typeof(UserRole);
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccount(SystemAccountDTO account)
		{
			var result = await _systemAccountService.UpdateAccount(account);
			if (result)
			{
				return Json(new { success = true, message = "Update successfully!", messageType = "success" });
			}
			else
			{
				return Json(new { success = false, message = "Update fail!", messageType = "error" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAccount(short id)
		{
			var account = await _systemAccountService.GetAccoutnProfile(id);
			if (account == null)
				return NotFound();

			return Json(account);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteAccount(short id)
		{
			var result = await _systemAccountService.DeleteAccount(id);
			if (result)
			{
				return Json(new { success = true, message = "Delete successfully!", messageType = "success" });
			}
			else
			{
				return Json(new { success = false, message = "Delete fail!", messageType = "error" });
			}
		}

		[HttpPost]

		public async Task<IActionResult> CreateAccount(SystemAccountDTO account)
		{
			var result = await _systemAccountService.CreateAccount(account);
			if (result)
			{
				return Json(new { success = true, message = "Create successfully!", messageType = "success" });
			}
			else
			{
				return Json(new { success = false, message = "Create fail!", messageType = "error" });
			}
		}

		[HttpPost]
		public async Task<IActionResult> Search(string search)
		{
			if (search == null)
			{
				return RedirectToAction("Index", "SystemAccount");
			}
			var result = await _systemAccountService.SearchByName(search);

			ViewBag.Search = result;
			ViewBag.SearchInput = search;
			ViewBag.Role = typeof(UserRole);
			return View("Index", result);
		}

		public async Task<IActionResult> Profile()
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
				var user = await _systemAccountService.GetAccoutnProfile(short.Parse(userId.Value));
				if (user == null)
				{
					return RedirectToAction("Login", "SystemAccount");
				}
				return View(user);
			}
			catch
			{
				return RedirectToAction("Login", "SystemAccount");
			}
		}
		[HttpPost]
		public async Task<IActionResult> UpdateProfile([FromForm] SystemAccountDTO accountDTO)
		{
			try
			{
				await _systemAccountService.UpdateProfile(accountDTO);
				return RedirectToAction("Profile", "SystemAccount");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("Login", "SystemAccount");
			}
		}

	}
}
