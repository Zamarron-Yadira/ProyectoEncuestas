using EncuestasAPI.Models;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.Validators;
using EncuestasAPI.Repositories;
using EncuestasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncuestasAPI.Controllers
{
	[AllowAnonymous]
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		public UsuariosController(Repository<Usuarios> repoUsuarios, UsuarioValidator validador, JWTService service)
		{
			RepoUsuarios = repoUsuarios;
			Validador = validador;
			Service = service;
		}
		public Repository<Usuarios> RepoUsuarios { get; }
		public UsuarioValidator Validador { get; }
		public JWTService Service { get; }

		[Authorize(Roles = "Admin")]
		[HttpPost("registrar")]
		public IActionResult Registrar(RegistrarUsuarioDTO dto)
		{
			if (Validador.ValidateRegister(dto, out List<string> errores))
			{
				Usuarios user = new()
				{
					Contrasena = dto.Contrasena,
					Nombre = dto.Nombre,
					FechaRegistro = DateTime.Now,
					EsAdmin = dto.EsAdmin == 1 ? 1 : 2 
				};
				RepoUsuarios.Insert(user);
				return Ok();
			}
			else
			{
				return BadRequest(errores);
			}
		}

		[HttpPost("login")]
		public IActionResult Login(LoginUsuarioDTO dto)
		{
			var token = Service.GenerarToken(dto);
			if (token == null)
			{
				return Unauthorized("El usuario o contraseña son incorrectos");
			}
			return Ok(token);
		}

		//AGREGAR: EDITAR CONTRASENA Y EDITAR USUARIO, ELIMINAR USUARIO (OPCIONAL)
	}
}
