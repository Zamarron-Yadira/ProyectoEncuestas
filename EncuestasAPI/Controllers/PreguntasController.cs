using EncuestasAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncuestasAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PreguntasController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetAll()
		{	
			return Ok();
		}
	}
}
