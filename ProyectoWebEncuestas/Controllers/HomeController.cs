using Microsoft.AspNetCore.Mvc;

namespace EncuestasWeb.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Login()
		{
			return View(); 
		}

		public IActionResult Index()
		{
			return View(); 
		}
	}
}
