using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using PRN232_SE1728_A01_FE.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace PRN232_SE1728_A01_FE.Controllers
{
	public class NewsArticleController : Controller
	{
		private readonly INewsArticleService _newsArticleService;
		private readonly ICategoryService _categoryService;
		private readonly ITagService _tagService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHttpContextAccessor httpContextAccessor)
		{
			_newsArticleService = newsArticleService;
			_categoryService = categoryService;
			_tagService = tagService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _newsArticleService.GetNewsArticlesList();
			if (!User.IsInRole("Staff"))
			{
				result = result.Where(s => s.NewsStatus == true).ToList();
			}

			if (User.IsInRole("Staff"))
			{
				ViewBag.Categories = await _categoryService.GetAllListAsync();
				ViewBag.Tags = await _tagService.GetListTags();
			}
			return View(result);
		}

		[HttpGet]
		public async Task<IActionResult> Detail(string id)
		{
			var result = await _newsArticleService.GetNewsById(id);
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> Search(string search)
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					return RedirectToAction(nameof(Index));
				}

				var result = await _newsArticleService.SearchByTittle(search);
				if (User.IsInRole("Staff"))
				{
					ViewBag.Categories = await _categoryService.GetAllListAsync();
					ViewBag.Tags = await _tagService.GetListTags();
				}

				ViewBag.Search = result;
				ViewBag.SearchInput = search;
				return View("Index", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewsArticle(NewsArticleDTO news)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return RedirectToAction("Login", "SystemAccount");
				}
				news.CreatedById = short.Parse(userId.Value);
				var test = news.Tags;
				news.NewsStatus = true;
				await _newsArticleService.CreateNewsArticle(news);
				return RedirectToAction("Index", "NewsArticle");
			}
			catch (Exception e)
			{
				return RedirectToAction("Index", "NewsArticle");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateNewsArticle(NewsArticleDTO editNews)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return RedirectToAction("Login", "SystemAccount");
				}
				if (editNews.NewsStatus == null)
				{
					editNews.NewsStatus = false;
				}
				var test = editNews.Tags;
				editNews.UpdatedById = short.Parse(userId.Value);

				await _newsArticleService.UpdateNews(editNews);
				return RedirectToAction("Index", "NewsArticle");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return RedirectToAction("Index", "NewsArticle");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetNewsArticle(string id)
		{
			var newsArticle = await _newsArticleService.GetNewsById(id);
			if (newsArticle == null)
				return NotFound();

			return Json(newsArticle);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteNews(string id)
		{
			try
			{
				var result = await _newsArticleService.DeleteNews(id);
				if (result)
				{
					return Json(new { success = true, message = "Delete successfully!", messageType = "success" });
				}
				else
				{
					return Json(new { success = false, message = "Delete fail!", messageType = "error" });
				}
			}
			catch
			{
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> NewsHistory()
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					return RedirectToAction("Login", "SystemAccount");
				}
				var result = await _newsArticleService.GetNewsArticlesHistory(short.Parse(userId.Value));
				return View(result);
			}
			catch
			{
				return View();
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> FilterByDate(DateTime fromDate, DateTime toDate)
		{
			try
			{
				var result = await _newsArticleService.FilterByDate(fromDate, toDate);
				TempData["FilteredData"] = JsonConvert.SerializeObject(result);
				TempData["FromDate"] = fromDate.ToString("yyyy-MM-dd");
				TempData["ToDate"] = toDate.ToString("yyyy-MM-dd");

				return View("Index", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return RedirectToAction("Index");
			}
		}
	}
}
