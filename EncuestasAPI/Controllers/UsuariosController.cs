using EncuestasAPI.Models;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.Validators;
using EncuestasAPI.Repositories;
using EncuestasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
					EsAdmin = dto.EsAdmin
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
		[HttpGet("{id}")]
		public IActionResult GetUsuarioPorId(int id)
		{
			var usuario = RepoUsuarios.GetId(id);

			if (usuario == null)
			{
				return NotFound(new { mensaje = "Usuario no encontrado." });
			}

			var dto = new UsuarioResumenDTO
			{
				Id = usuario.Id,
				Nombre = usuario.Nombre,
				FechaRegistro = usuario.FechaRegistro,
				EsAdmin = usuario.EsAdmin
			};

			return Ok(dto);
		}


		[Authorize(Roles = "Admin")]
		[HttpGet("usuarios")]
		public IActionResult GetUsuarios()
		{
			var usuarios = RepoUsuarios.GetAll();

			var lista = usuarios.Select(u => new UsuarioResumenDTO
			{
				Id = u.Id,
				Nombre = u.Nombre,
				FechaRegistro = u.FechaRegistro,
				EsAdmin = u.EsAdmin
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
        public IActionResult Login([FromBody] LoginUsuarioDTO dto)
        {
	       if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Contrasena))
	       {
			 return BadRequest("El nombre de usuario y contraseña son obligatorios.");
	        }


			//var usuario = RepoUsuarios.GetAll().FirstOrDefault(u => u.Nombre == dto.Nombre && u.Contrasena == dto.Contrasena);
			var usuario = RepoUsuarios.GetAll().FirstOrDefault(u =>
				u.Nombre.Trim().ToLower() == dto.Nombre.Trim().ToLower() &&
				u.Contrasena.Trim() == dto.Contrasena.Trim());

			if (usuario == null)
			{
				return Unauthorized("El usuario o contraseña son incorrectos");
			}

			try
			{
				var token = Service.GenerarToken(usuario);
				if (token == null)
				{
					return StatusCode(500, "No se pudo generar el token");
				}

				return Ok(new
				{
					token,
					esAdmin = usuario.EsAdmin ?? "Usuario",
					id = usuario.Id,
					nombre = usuario.Nombre
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error generando token: {ex.Message}");
			}
		}


	}
}
