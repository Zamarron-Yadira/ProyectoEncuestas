using AutoMapper;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.Validators;
using EncuestasAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

		public PreguntasController(Repository<Preguntas> preguntaRepo,
		IMapper mapper, EncuestaValidator validador,  EstadisticasRepository estadisticasRepo)
		{
			_preguntaRepo = preguntaRepo;
			_mapper = mapper;
			Validador = validador;
			EstadisticasRepo = estadisticasRepo;
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

		[Authorize(Roles = "Admin")]
		[HttpGet("estadisticas/totalpreguntas")]
		public IActionResult GetTotalPreguntas()
		{
			return Ok(EstadisticasRepo.GetTotalPreguntas());
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("respuestas-preguntas")]
		public ActionResult<List<RespuestasPorPreguntasDTO>> GetRespuestasPorPregunta()
		{
			var data = EstadisticasRepo.GetCantidadRespuestasPorPregunta();
			return Ok(data);
		}

		//FALTA AGREGAR EL CONTROLLADOR DE RESPUESTAS Y AGREGAR SIGNAL R
		//AGREGAR EDITAR Y ELIMINAR PREGUNTAS
	}
}
