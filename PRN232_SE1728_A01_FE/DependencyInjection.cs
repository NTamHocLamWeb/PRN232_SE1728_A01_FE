using Microsoft.AspNetCore.Authentication.Cookies;
using PRN232_SE1728_A01_FE.Services.Interfaces;
using PRN232_SE1728_A01_FE.Services.Services;

namespace PRN232_SE1728_A01_FE
{
	public static class DependencyInjection
	{
		public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddServices();
			services.AddAuthentication();
		}

		public static void AddServices(this IServiceCollection services)
		{

			services.AddHttpClient<ISystemAccountService, SystemAccountService>();
			services.AddHttpClient<INewsArticleService, NewsArticleService>();
			services.AddHttpClient<ICategoryService, CategoryService>();
			services.AddHttpClient<ITagService, TagService>();
			services.AddHttpContextAccessor();
		}

		public static void AddAuthentication(this IServiceCollection services)
		{
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.LoginPath = "/SystemAccount/Login";
				options.AccessDeniedPath = "/SystemAccount/Login";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
			});
		}
	}
}
