using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EncuestasAPI.Services
{
	public class JWTService
	{
		public JWTService(IConfiguration configuration, Repository<Usuarios> reposUsuarios)
		{
			Configuration = configuration;
			ReposUsuarios = reposUsuarios;
		}

		public IConfiguration Configuration { get; }
		public Repository<Usuarios> ReposUsuarios { get; }

		public string? GenerarToken(LoginUsuarioDTO dto)
		{
			//buscar si existe el usuario en la base de datos
			var usuario = ReposUsuarios.GetAll().FirstOrDefault(x => x.Nombre == dto.Nombre && x.Contrasena == dto.Contrasena);
			if (usuario == null)
			{
				return null; // Usuario no encontrado o credenciales incorrectas
			}
			else
			{
				//1. Crear claims
				List<Claim> claims = new List<Claim>
				{
					new Claim("Id", usuario.Id.ToString()),
					new Claim(ClaimTypes.Name, usuario.Nombre),
					new Claim(ClaimTypes.Role, dto.EsAdmin == 1? "Admin":"Usuario")
				};

				//2. Crear un descriptor de token con esos claims

				var descriptor = new JwtSecurityToken(
					issuer: Configuration["Jwt:Issuer"],
					audience: Configuration["Jwt:Audience"],
					claims: claims,
					notBefore: DateTime.UtcNow,
					expires: DateTime.UtcNow.AddMinutes(10), // Expira en 30 minutos
					signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
						System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
					);

				//3. Crear JWT

				var handler = new JwtSecurityTokenHandler();
				return handler.WriteToken(descriptor);
			}
		}
	}
}