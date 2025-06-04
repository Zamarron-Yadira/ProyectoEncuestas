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
	
	public class PreguntasController : ControllerBase
	{
		private readonly Repository<Preguntas> _preguntaRepo;

		private readonly IMapper _mapper;

		public EncuestaValidator Validador { get; }
		public EstadisticasRepository EstadisticasRepo { get; }

		private readonly IHubContext<EstadisticasHub> _hub;

		public PreguntasController(Repository<Preguntas> preguntaRepo,
		IMapper mapper, EncuestaValidator validador,  
		EstadisticasRepository estadisticasRepo, IHubContext<EstadisticasHub> hub)
		{
			_preguntaRepo = preguntaRepo;
			_mapper = mapper;
			Validador = validador;
			EstadisticasRepo = estadisticasRepo;
		  _hub = hub;
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
		public async Task <IActionResult> EditarPregunta(int id, [FromBody] EditarPreguntaDTO dto)
		{
			var pregunta = _preguntaRepo.GetId(id);
			if (pregunta == null)
			{
				return NotFound("La pregunta no existe.");
			}

			pregunta.Descripcion = dto.Descripcion;
			pregunta.NumeroPregunta = dto.NumeroPregunta;

			_preguntaRepo.Update(pregunta);
			await _hub.Clients.All.SendAsync("ActualizarEstadisticas");

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
