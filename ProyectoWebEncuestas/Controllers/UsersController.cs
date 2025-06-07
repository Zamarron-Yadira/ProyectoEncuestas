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
		public IActionResult AgregarEncuesta() {
			return View();
		}
		public IActionResult EditarEncuesta()
		{
			return View();
		}
		public IActionResult EliminarEncuesta()
		{
			return View();
		}
		public IActionResult DetalleEncuesta()
		{
			return View();
		}
		public IActionResult AgregarRespuestaAlumno()
		{
			return View();
		}
		public IActionResult ResponderPreguntas()
		{
			return View();
		}
	}
}
