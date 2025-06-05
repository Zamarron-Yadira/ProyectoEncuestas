using Microsoft.AspNetCore.Mvc;

namespace EncuestasWeb.Controllers
{
	public class UsersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult VerMisEncuestas()
		{
			return View();
		}
	}
}
