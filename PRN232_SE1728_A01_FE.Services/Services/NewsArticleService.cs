using Newtonsoft.Json;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using System.Security.Principal;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PRN232_SE1728_A01_FE.Services.Services
{
	public class NewsArticleService : INewsArticleService
	{
		private readonly HttpClient _httpClient;
		private readonly ISystemAccountService _systemAccountService;

		public NewsArticleService(HttpClient httpClient, ISystemAccountService systemAccountService)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7193");
			_systemAccountService = systemAccountService;
		}

		public async Task<bool> CreateNewsArticle(NewsArticleDTO newNewsArticleDTO)
		{
			var listtags = newNewsArticleDTO.Tags.Where(s => s.TagId != 0).Select(s => s.TagId).ToList();
			var payload = new
			{
				newsTitle = newNewsArticleDTO.NewsTitle,
				headline = newNewsArticleDTO.Headline,
				newsContent = newNewsArticleDTO.NewsContent,
				newsSource = newNewsArticleDTO.NewsSource,
				createdById = newNewsArticleDTO.CreatedById,
				categoryId = newNewsArticleDTO.CategoryId,
				tagIds = listtags,
			};

			var request = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("/odata/NewsArticles", request);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteNews(string id)
		{
			var newId = int.Parse(id);
			var response = await _httpClient.DeleteAsync($"/odata/NewsArticles/{newId}");

			return response.IsSuccessStatusCode;
		}

		public async Task<List<NewsArticleDTO>> FilterByDate(DateTime fromDate, DateTime toDate)
		{
			string fromDateString = fromDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
			string toDateString = toDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
			string orderBy = Uri.EscapeDataString("CreatedDate desc");

			string filter = Uri.EscapeDataString($"CreatedDate ge {fromDateString} and CreatedDate le {toDateString}");
			var expand = Uri.EscapeDataString("Tags");

			var response = await _httpClient.GetAsync($"/odata/NewsArticles?$filter={filter}&$expand={expand}&$orderby={orderBy}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleDTO>>(content);

			var result = odataResponse.Value;
			foreach (var newsArticleDTO in result)
			{
				if (newsArticleDTO.UpdatedById.HasValue)
				{
					var account = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.UpdatedById.Value);
					newsArticleDTO.UpdatedBy = account?.AccountName ?? "Unknown";
				}
				else
				{
					newsArticleDTO.UpdatedBy = "Unknown";
				}
				var create = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.CreatedById.Value);
				newsArticleDTO.CreatedBy = create?.AccountName ?? "Unknown";
			}

			return result;
		}

		public async Task<List<NewsArticleDTO>> GetNewsArticlesHistory(short id)
		{
			var filter = Uri.EscapeDataString($"CreatedById eq {id}");
			var expand = Uri.EscapeDataString("Tags");
			string orderBy = Uri.EscapeDataString("ModifiedDate desc");

			var response = await _httpClient.GetAsync($"/odata/NewsArticles?$filter={filter}&$expand={expand}&$orderby={orderBy}");


			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleDTO>>(content);

			var result = odataResponse.Value;
			foreach (var newsArticleDTO in result)
			{
				if (newsArticleDTO.UpdatedById.HasValue)
				{
					var account = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.UpdatedById.Value);
					newsArticleDTO.UpdatedBy = account?.AccountName ?? "Unknown";
				}
				else
				{
					newsArticleDTO.UpdatedBy = "Unknown";
				}
				var create = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.CreatedById.Value);
				newsArticleDTO.CreatedBy = create?.AccountName ?? "Unknown";
			}

			return result;
		}

		public async Task<List<NewsArticleDTO>> GetNewsArticlesList()
		{
			string orderBy = Uri.EscapeDataString("ModifiedDate desc");
			var response = await _httpClient.GetAsync($"/odata/NewsArticles?$expand=Tags&$orderby={orderBy}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleDTO>>(content);

			var result = odataResponse.Value;
			foreach (var newsArticleDTO in result)
			{
				if (newsArticleDTO.UpdatedById.HasValue)
				{
					var account = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.UpdatedById.Value);
					newsArticleDTO.UpdatedBy = account?.AccountName ?? "Unknown";
				}
				else
				{
					newsArticleDTO.UpdatedBy = "Unknown";
				}
				var create = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.CreatedById.Value);
				newsArticleDTO.CreatedBy = create?.AccountName ?? "Unknown";
			}

			return result;
		}

		public async Task<NewsArticleDTO> GetNewsById(string id)
		{
			var filter = Uri.EscapeDataString($"NewsArticleId eq '{id}'");
			var expand = Uri.EscapeDataString("Tags");

			var response = await _httpClient.GetAsync($"/odata/NewsArticles?$filter={filter}&$expand={expand}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleDTO>>(content);

			var result = odataResponse.Value.FirstOrDefault();
			if (result != null)
			{
				if (result.UpdatedById.HasValue)
				{
					var account = await _systemAccountService.GetAccoutnProfile(result.UpdatedById.Value);
					result.UpdatedBy = account?.AccountName ?? "Unknown";
				}
				else
				{
					result.UpdatedBy = "Unknown";
				}
				var create = await _systemAccountService.GetAccoutnProfile(result.CreatedById.Value);
				result.CreatedBy = create?.AccountName ?? "Unknown";
			}

			return result;
		}

		public async Task<List<NewsArticleDTO>> SearchByTittle(string search)
		{
			var filter = Uri.EscapeDataString($"contains(tolower(NewsTitle), '{search.ToLower()}')");
			var expand = Uri.EscapeDataString("Tags");
			string orderBy = Uri.EscapeDataString("ModifiedDate desc");

			var response = await _httpClient.GetAsync($"/odata/NewsArticles?$filter={filter}&$expand={expand}&$orderby={orderBy}");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleDTO>>(content);
			var result = odataResponse.Value;

			foreach (var newsArticleDTO in result)
			{
				if (newsArticleDTO.UpdatedById.HasValue)
				{
					var account = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.UpdatedById.Value);
					newsArticleDTO.UpdatedBy = account?.AccountName ?? "Unknown";
				}
				else
				{
					newsArticleDTO.UpdatedBy = "Unknown";
				}
				var create = await _systemAccountService.GetAccoutnProfile(newsArticleDTO.CreatedById.Value);
				newsArticleDTO.CreatedBy = create?.AccountName ?? "Unknown";
			}

			return result;
		}

		public async Task<bool> UpdateNews(NewsArticleDTO editNews)
		{
			var updateData = new
			{
				newsTitle = editNews.NewsTitle,
				headline = editNews.Headline,
				newsContent = editNews.NewsContent,
				newsSource = editNews.NewsSource,
				categoryId = editNews.CategoryId,
				newsStatus = editNews.NewsStatus,
				tags = editNews.Tags
			};

			var request = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

			var response = await _httpClient.PatchAsync($"/odata/NewsArticles/{editNews.NewsArticleId}", request);

			return response.IsSuccessStatusCode;
		}
	}
}
