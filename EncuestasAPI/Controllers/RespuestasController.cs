using AutoMapper;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
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
		//CHECAR RESPUESTA GETALL

		public RespuestasController(Repository<RespuestaDTO> respuestasRepo, IMapper mapper)
		{
			RespuestasRepo = respuestasRepo;
			Mapper = mapper;
		}


		//AGREGAR LA RESPUESTA DONDE PIDE EL NOMBRE Y NUM CONTROL
		public Repository<RespuestaDTO> RespuestasRepo { get; }
		public IMapper Mapper { get; }



		[HttpPost("responderEncuesta")] //Aqui pide el nombre y el numero de control
		public IActionResult ResponderEmcuesta([FromBody] RespuestaDetalleDTO dto)
		{

			return Ok();
		}

		[HttpPost("responderPregunta")]//Aqui responde cada pregunta
		public IActionResult ResponderPregunta([FromBody] RespuestaDetalleDTO dto)
		{
			
			return Ok();
		}
		[HttpPost("editarrespuestas")]
		public IActionResult EditarRespuesta([FromBody] RespuestaDetalleDTO dto)
		{
		
			return Ok();
		}


	}
}
