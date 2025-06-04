using AutoMapper;
using EncuestasAPI.Hubs;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EncuestasAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class RespuestasController : ControllerBase
	{
		private readonly IHubContext<EstadisticasHub> _hub;
		public RespuestasController(IMapper mapper, Repository<Detallerespuestas> repoDetalles,
			Repository<Respuestas> _respuestasRepository, IHubContext<EstadisticasHub> hub)
		{
			Mapper = mapper;
			RepoDetalles = repoDetalles;
			RespuestasRepository = _respuestasRepository;
			_hub = hub;
			
		}

		public IMapper Mapper { get; }
		public Repository<Detallerespuestas> RepoDetalles { get; }
		public Repository<Respuestas> RespuestasRepository { get; }

		[HttpGet("todas")]
		public IActionResult GetAll()
		{
			var respuestas = RespuestasRepository.GetAll()
				.Select(r => new
				{
					r.Id,
					r.NombreAlumno,
					r.NumControlAlumno,
					r.FechaAplicacion,
					Respuestas = r.Detallerespuestas.Select(d => new
					{
						d.IdPregunta,
						d.ValorEvaluacion
					})
				});
			return Ok(respuestas);
		}

		[HttpPost("registrarInicio")]
		public async Task <IActionResult> RegistrarInicio([FromBody] RespuestaDTO dto)
		{
			var entidad = Mapper.Map<Respuestas>(dto);
			entidad.FechaAplicacion = DateTime.Now;

			RespuestasRepository.Insert(entidad);

			await _hub.Clients.All.SendAsync("ActualizarEstadisticas");

			return Ok(new
			{
				mensaje = "Datos del alumno registrados correctamente.",
				idRespuesta = entidad.Id
			});
		}


		//RESPONDER TODAS:
		[HttpPost("responderPreguntas")]
		public async Task <IActionResult> ResponderPreguntas([FromBody] List<RespuestaDetalleDTO> dtos)
		{
			if (dtos == null || dtos.Count == 0)
			{
				return BadRequest("No se recibieron respuestas.");
			}

			foreach (var dto in dtos)
			{
				var detalleRespuesta = new Detallerespuestas
				{
					IdRespuesta = dto.IdRespuesta,
					IdPregunta = dto.IdPregunta,
					ValorEvaluacion = dto.ValorEvaluacion
				};

				// Guarda cada detalle de respuesta
				RepoDetalles.Insert(detalleRespuesta);
			}
			await _hub.Clients.All.SendAsync("ActualizarEstadisticas");

			return Ok(new
			{
				mensaje = "Respuestas registradas correctamente.",
				cantidad = dtos.Count
			});
		}

	}
}
