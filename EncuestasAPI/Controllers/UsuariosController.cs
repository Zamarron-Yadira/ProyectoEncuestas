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
	[Authorize]
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
		[Authorize(Roles = "Admin")]
		[HttpGet("usuarios")]
		public IActionResult GetUsuarios()
		{
			var usuarios = RepoUsuarios.GetAll();

			var lista = usuarios.Select(u => new UsuarioResumenDTO
			{
				Nombre = u.Nombre,
				FechaRegistro = u.FechaRegistro,
				EsAdmin = u.EsAdmin == 1
			}).ToList();

			return Ok(lista);
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public IActionResult EliminarUsuario(int id)
		{
			var usuario = RepoUsuarios.GetId(id);
			if (usuario == null)
			{
				return NotFound(new { mensaje = "Usuario no encontrado." });
			}

			var currentId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
			if (id == currentId)
				return BadRequest("No puedes eliminar tu propio usuario.");

			try
			{
				RepoUsuarios.Delete(id);
				return Ok(new { mensaje = "Usuario eliminado correctamente." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { mensaje = "Error al eliminar el usuario.", detalle = ex.Message });
			}
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login(LoginUsuarioDTO dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Contrasena))
			{
				return BadRequest("El nombre de usuario y contraseña son obligatorios.");
			}
			var token = Service.GenerarToken(dto);
			if (token == null)
			{
				return Unauthorized("El usuario o contraseña son incorrectos");
			}
			return Ok(token);
		}

	}
}
