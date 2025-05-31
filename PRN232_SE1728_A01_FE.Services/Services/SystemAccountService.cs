using Newtonsoft.Json;
using PRN232_SE1728_A01_FE.Services.DTOs;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using System.Security.Principal;
using System.Text;

namespace PRN232_SE1728_A01_FE.Services.Services
{
	public class SystemAccountService : ISystemAccountService
	{
		private readonly HttpClient _httpClient;

		public SystemAccountService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://localhost:7193");
		}

		public async Task<SystemAccountDTO> GetSystemAccountAsync(string email, string password)
		{
			var payload = new
			{
				email = email,
				password = password
			};

			var request = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("/signin", request);

			response.EnsureSuccessStatusCode();


			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<SystemAccountDTO>(content);
			return odataResponse;
		}

		public async Task<SystemAccountDTO> GetAccoutnProfile(short id)
		{
			var filter = Uri.EscapeDataString($"AccountId eq {id}");

			var response = await _httpClient.GetAsync($"/odata/SystemAccounts?$filter={filter}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<SystemAccountDTO>>(content);

			return odataResponse.Value.FirstOrDefault();
		}

		public async Task<List<SystemAccountDTO>> GetListAccount()
		{
			var response = await _httpClient.GetAsync("/odata/SystemAccounts");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<SystemAccountDTO>>(content);

			return odataResponse.Value;
		}

		public async Task<bool> UpdateAccount(SystemAccountDTO account)
		{
			var updateData = new
			{
				accountName = account.AccountName,
				accountEmail = account.AccountEmail,
				accountRole = account.AccountRole,
			};

			var request = new StringContent(JsonConvert.SerializeObject(updateData),Encoding.UTF8, "application/json");

			var response = await _httpClient.PatchAsync($"/odata/SystemAccounts/{account.AccountId}", request);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> CreateAccount(SystemAccountDTO account)
		{
			var payload = new
			{
				accountName = account.AccountName,
				accountEmail = account.AccountEmail,
				accountRole = account.AccountRole,
				accountPassword = account.AccountPassword,
			};

			var request = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync("/odata/SystemAccounts", request);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteAccount(short id)
		{
			var response = await _httpClient.DeleteAsync($"/odata/SystemAccounts/{id}");

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateProfile(SystemAccountDTO profile)
		{
			var updateData = new
			{
				accountName = profile.AccountName,
				accountEmail = profile.AccountEmail,
				accountPassword = profile.AccountPassword,
				accountRole = profile.AccountRole,
			};

			var request = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

			var response = await _httpClient.PatchAsync($"/odata/SystemAccounts/{profile.AccountId}", request);

			return response.IsSuccessStatusCode;
		}

		public async Task<List<SystemAccountDTO>> SearchByName(string name)
		{
			var filter = Uri.EscapeDataString($"contains(tolower(AccountName), '{name.ToLower()}')");
			var response = await _httpClient.GetAsync($"/odata/SystemAccounts?$filter={filter}");

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			var odataResponse = JsonConvert.DeserializeObject<ODataResponse<SystemAccountDTO>>(content);

			return odataResponse.Value;
		}
	}
}
