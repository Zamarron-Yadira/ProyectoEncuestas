using AutoMapper;
using EncuestasAPI.Hubs;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.Validators;
using EncuestasAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EncuestasAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	//Agregar Signar r: para ver que usuarios estan aplicando las encuestas em ese momento, ademas de ver que encuesta estan aplicando

	public class PreguntasController : ControllerBase
	{
		private readonly Repository<Preguntas> _preguntaRepo;

		private readonly IMapper _mapper;

		public EncuestaValidator Validador { get; }
		public EstadisticasRepository EstadisticasRepo { get; }
		public EstadisticasHub Hub { get; }

		public PreguntasController(Repository<Preguntas> preguntaRepo,
		IMapper mapper, EncuestaValidator validador,  
		EstadisticasRepository estadisticasRepo, EstadisticasHub hub)
		{
			_preguntaRepo = preguntaRepo;
			_mapper = mapper;
			Validador = validador;
			EstadisticasRepo = estadisticasRepo;
			Hub = hub;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var preguntas = _preguntaRepo.GetAll();
			var dto = _mapper.Map<IEnumerable<PreguntaDTO>>(preguntas);
			return Ok(dto);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var pregunta = _preguntaRepo.GetId(id);
			if (pregunta == null)
			{
				return NotFound();
			}
			var dto = _mapper.Map<PreguntaDTO>(pregunta);
			return Ok(dto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditarPregunta(int id, [FromBody] EditarPreguntaDTO dto)
		{
			var pregunta = _preguntaRepo.GetId(id);
			if (pregunta == null)
			{
				return NotFound("La pregunta no existe.");
			}

			pregunta.Descripcion = dto.Descripcion;
			pregunta.NumeroPregunta = dto.NumeroPregunta;

			_preguntaRepo.Update(pregunta);
			await Hub.Clients.All.SendAsync("ActualizarEstadisticas");

			return Ok("Pregunta actualizada correctamente.");
		}
		
		


		[Authorize(Roles = "Admin")]
		[HttpGet("estadisticas/totalpreguntas")]
		public IActionResult GetTotalPreguntas()
		{
			return Ok(EstadisticasRepo.GetTotalPreguntas());
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("respuestasxpreguntas")]
		public ActionResult<List<RespuestasPorPreguntasDTO>> GetRespuestasPorPregunta()
		{
			var data = EstadisticasRepo.GetCantidadRespuestasPorPregunta();
			return Ok(data);
		}		

	}
}
