using Microsoft.AspNetCore.Mvc;

namespace EncuestasWeb.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult DetalleEncuestas()
		{
			return View();
		}
		public IActionResult Encuestas()
		{
			return View();
		}
		public IActionResult Usuarios()
		{
			return View();
		}
		public IActionResult AgregarUsuario()
		{
			return View();
		}
		public IActionResult EliminarUsuario()
		{
			return View();
		}
	}
}
