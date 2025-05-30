using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncuestasAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class RespuestasController : ControllerBase
	{
		//AGREGAR RESPUESTA Y EDITAR, VER TODAS LAS RESPUESTAS Y por ID DE PREGUNTA
		
	}
}
