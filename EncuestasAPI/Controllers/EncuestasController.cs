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
using Microsoft.EntityFrameworkCore;

namespace EncuestasAPI.Controllers
{

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class EncuestasController : ControllerBase
	{
		//Agregar Signar r: para ver que usuarios estan aplicando las encuestas em ese momento, ademas de ver que encuesta estan aplicando

		private readonly Repository<Encuestas> _encuestaRepo;
		private readonly Repository<Preguntas> _preguntaRepo;
		private readonly EstadisticasRepository _estadisticasRepo;

		private readonly IMapper _mapper;

		public EncuestaValidator Validador { get; }
		public IHubContext<EstadisticasHub> Hub { get; }

		public EncuestasController(Repository<Encuestas> encuestaRepo,
		Repository<Preguntas> preguntaRepo,
		IMapper mapper, EncuestaValidator validador, EstadisticasRepository estadisticasRepo,
		IHubContext<EstadisticasHub> hub)
		{
			_encuestaRepo = encuestaRepo;
			_preguntaRepo = preguntaRepo;
			_estadisticasRepo = estadisticasRepo;
			Hub = hub;
			_mapper = mapper;
			Validador = validador;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var encuestas = _encuestaRepo.GetAll();
			var dto = _mapper.Map<IEnumerable<EncuestaDTO>>(encuestas);
			return Ok(dto);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var encuesta = _encuestaRepo.GetId(id);
			if (encuesta == null)
			{
				return NotFound();
			}
			var dto = _mapper.Map<EncuestaDTO>(encuesta);
			return Ok(dto);
		}

		[HttpPost("crear")]
		public async Task<IActionResult> CreateEncuesta([FromBody] CrearEncuestaDTO dto)
		{
			if (Validador.Validate(dto, out List<string> errores))
			{
				var encuesta = new Encuestas
				{
					IdUsuario = dto.IdUsuario,
					Titulo = dto.Titulo,
					FechaCreacion = DateTime.Now,
					Preguntas = dto.Preguntas.Select (P=> new Preguntas
					{
						Descripcion = P.Descripcion,
						NumeroPregunta = P.NumeroPregunta
					}).ToList()
				};
				_encuestaRepo.Insert(encuesta);

				var encuestaDto = new EncuestaDTO
				{
					Id = encuesta.Id,
					IdUsuario = encuesta.IdUsuario,
					Titulo = encuesta.Titulo,
					FechaCreacion = encuesta.FechaCreacion
				};

				await Hub.Clients.All.SendAsync("ActualizarEstadisticas");

				return Ok(new
				{
					mensaje = "Encuesta creada correctamente.",
					encuesta = encuestaDto
				});
			}
			else
			{
				return BadRequest(errores);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditarEncuesta(int id, [FromBody] EditarEncuestaDTO dto)
		{
			var encuesta = _encuestaRepo.GetId(id);
			if (encuesta == null)
				return NotFound("La encuesta no existe.");

			encuesta.Titulo = dto.Titulo;
			_encuestaRepo.Update(encuesta);

			await Hub.Clients.All.SendAsync("ActualizarEstadisticas");

			return Ok("Encuesta actualizada correctamente.");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> EliminarEncuesta(int id)
		{
			var encuesta = _encuestaRepo.GetId(id);
			if (encuesta == null)
				return NotFound("La encuesta no existe.");

			_encuestaRepo.Delete(id);

			await Hub.Clients.All.SendAsync("ActualizarEstadisticas");

			return Ok("Encuesta eliminada correctamente.");
		}

		[Authorize(Roles ="Admin")]
		[HttpGet("estadisticas/totalencuestas")]
		public IActionResult GetTotalEncuestas()
		{
			return Ok(_estadisticasRepo.GetTotalEncuestasCreadas());
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("estadisticas/totalrespondidas")]
		public IActionResult GetTotalEncuestasRespondidas()
		{
			return Ok(_estadisticasRepo.GetTotalEncuestasRespondidas());
		}
		[Authorize(Roles = "Admin")]
		[HttpGet("estadisticas/totalnorespondidas")]
		public IActionResult GetTotalEncuestasSinResponder ()
		{
			return Ok(_estadisticasRepo.GetTotalEncuestasSinResponder());
		}


	}
}
