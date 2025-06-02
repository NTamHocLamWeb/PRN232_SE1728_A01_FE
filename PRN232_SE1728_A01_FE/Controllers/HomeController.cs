using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PRN232_SE1728_A01_FE.Controllers
{
	public class HomeController : Controller
	{
		// GET: HomeController
		public ActionResult Privacy()
		{
			return View();
		}
	}
}
