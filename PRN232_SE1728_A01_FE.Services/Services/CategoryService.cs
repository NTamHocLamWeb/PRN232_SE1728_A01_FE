using Newtonsoft.Json;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using System.Text;

namespace PRN232_SE1728_A01_FE.Services.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly HttpClient _httpClient;

		public CategoryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7193");
		}
		public async Task<bool> CreateCategory(CategoryDTO category)
		{
			var payload = new
			{
				categoryName = category.CategoryName,
				categoryDesciption = category.CategoryDesciption,
				parentCategoryId = category.ParentCategoryId,
			};

			var request = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("/odata/Categories", request);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteCategory(short id)
		{
			var response = await _httpClient.DeleteAsync($"/odata/Categories/{id}");

			return response.IsSuccessStatusCode;
		}

		public async Task<List<CategoryDTO>> GetAllListAsync()
		{
			var response = await _httpClient.GetAsync("/odata/Categories");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<CategoryDTO>>(content);

			var result = odataResponse.Value;
			foreach (var categoryDTO in result)
			{
				if (categoryDTO.ParentCategoryId.HasValue)
				{
					var category = await GetCategoryByid(categoryDTO.ParentCategoryId.Value);
					categoryDTO.ParentCategory = category?.CategoryName ?? "Unknown";
				}
			}

			return result;
		}

		public async Task<CategoryDTO> GetCategoryByid(short id)
		{
			var filter = Uri.EscapeDataString($"CategoryId eq {id}");

			var response = await _httpClient.GetAsync($"/odata/Categories?$filter={filter}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<CategoryDTO>>(content);

			var result = odataResponse.Value.FirstOrDefault();

			return result;
		}

		public async Task<List<CategoryDTO>> SearchByName(string name)
		{
			var filter = Uri.EscapeDataString($"contains(tolower(CategoryName), '{name.ToLower()}')");
			var response = await _httpClient.GetAsync($"/odata/Categories?$filter={filter}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<CategoryDTO>>(content);

			return odataResponse.Value;
		}

		public async Task<bool> UpdateCategory(CategoryDTO category)
		{
			var updateData = new
			{
				categoryName = category.CategoryName,
				categoryDesciption = category.CategoryDesciption,
				parentCategoryId = category.ParentCategoryId,
				isActive = category.IsActive,
			};

			var request = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

			var response = await _httpClient.PatchAsync($"/odata/Categories({category.CategoryId})", request);

			return response.IsSuccessStatusCode;
		}
	}
}
