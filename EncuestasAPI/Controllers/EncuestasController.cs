using AutoMapper;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncuestasAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EncuestasController : ControllerBase
	{
		private readonly Repository<Encuestas> _encuestaRepo;
		private readonly Repository<Preguntas> _preguntaRepo;

		private readonly IMapper _mapper;

		public EncuestasController(Repository<Encuestas> encuestaRepo,
		Repository<Preguntas> preguntaRepo,
		IMapper mapper)
		{
			_encuestaRepo = encuestaRepo;
			_preguntaRepo = preguntaRepo;
			_mapper = mapper;
		}

		//api/encuestas
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

		[HttpPost]
		public IActionResult CreateEncuesta()
		{
			return Ok();
		}
	}
}
