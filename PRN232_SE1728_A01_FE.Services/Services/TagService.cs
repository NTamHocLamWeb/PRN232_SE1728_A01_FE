using Newtonsoft.Json;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;

namespace PRN232_SE1728_A01_FE.Services.Services
{
	public class TagService : ITagService
	{
		private readonly HttpClient _httpClient;

		public TagService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7193");
		}
		public async Task<List<TagDTO>> GetListTags()
		{
			var response = await _httpClient.GetAsync($"/odata/Tags");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<TagDTO>>(content);

			var result = odataResponse.Value;

			return result;
		}
	}
}
