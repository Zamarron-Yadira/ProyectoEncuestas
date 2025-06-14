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
		private readonly EstadisticasRepository _estadisticasRepo;

		private readonly IHubContext<EstadisticasHub> _hub;
		public RespuestasController(IMapper mapper, Repository<Detallerespuestas> repoDetalles,
			Repository<Respuestas> _respuestasRepository, IHubContext<EstadisticasHub> hub,
			EstadisticasRepository estadisticasRepo)
		{
			Mapper = mapper;
			RepoDetalles = repoDetalles;
			RespuestasRepository = _respuestasRepository;
			_estadisticasRepo = estadisticasRepo;
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
			var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{3}[A-Za-z]{1}[0-9]{4}$");
			if (!regex.IsMatch(dto.NumControlAlumno))
			{
				return BadRequest("Por favor verifique su número de control.");
			}
			// Validar si ya respondió la encuesta
			var yaRespondio = RespuestasRepository.GetAll().Any(r =>
				r.IdEncuesta == dto.IdEncuesta &&
				r.NumControlAlumno == dto.NumControlAlumno);

			if (yaRespondio)
				return BadRequest("Este alumno ya ha respondido esta encuesta.");

			// Obtener el ID del usuario aplicador desde los claims del token
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
			if (userIdClaim == null)
				return Unauthorized("Token inválido o expirado.");

			dto.IdUsuarioAplicador = int.Parse(userIdClaim.Value);


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
			try
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
			catch (Exception ex)
			{
				return StatusCode(500, $"Error del servidor: {ex.Message}");
			}
		}

		[HttpDelete("cancelar/{idRespuesta}")]
		public IActionResult CancelarRespuesta(int idRespuesta)
		{
			var respuesta = RespuestasRepository.GetId(idRespuesta);
			if (respuesta == null)
				return NotFound("Registro no encontrado.");

			
			RespuestasRepository.Delete(idRespuesta);

			return Ok(new { mensaje = "Respuesta cancelada correctamente." });
		}

	}
}
