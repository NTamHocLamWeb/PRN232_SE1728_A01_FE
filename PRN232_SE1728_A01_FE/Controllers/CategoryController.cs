using Microsoft.AspNetCore.Mvc;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Enums;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using PRN232_SE1728_A01_FE.Services.Services;

namespace PRN232_SE1728_A01_FE.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var result = await _categoryService.GetAllListAsync();
			return View(result);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory(CategoryDTO editCategory)
		{
			try
			{
				if (editCategory.IsActive == null)
				{
					editCategory.IsActive = false;
				}
				await _categoryService.UpdateCategory(editCategory);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetCategory(short id)
		{
			var category = await _categoryService.GetCategoryByid(id);
			if (category == null)
				return NotFound();

			return Json(category);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteCategory(short id)
		{
			try
			{
				var result = await _categoryService.DeleteCategory(id);
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

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CategoryDTO category)
		{
			await _categoryService.CreateCategory(category);
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Search(string search)
		{
			if (search == null)
			{
				return RedirectToAction(nameof(Index));
			}
			var result = await _categoryService.SearchByName(search);

			ViewBag.Search = result;
			ViewBag.SearchInput = search;
			return View("Index", result);
		}
	}
}
