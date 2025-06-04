using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using System.Text.RegularExpressions;

namespace EncuestasAPI.Models.Validators
{
	public class UsuarioValidator
	{
		private readonly Repository<Usuarios> repoUsuario;

		public UsuarioValidator(Repository<Usuarios> repoUsuario)
		{
			this.repoUsuario = repoUsuario;
		}

		public bool Validate(LoginUsuarioDTO dto, out List<string> errores)
		{
			errores = new List<string>();
			if (string.IsNullOrWhiteSpace(dto.Nombre))
			{
				errores.Add("El nombre de usuario está vacío.");
			}
			else if (dto.Nombre.Length > 45)
			{
				errores.Add("El nombre de usuario debe tener una longitud máxima de 45 carácteres.");
			}

			if (string.IsNullOrWhiteSpace(dto.Contrasena))
			{
				errores.Add("La contraseña está vacía.");
			}
			if (dto.Contrasena.Length >= 8)
			{
				errores.Add("La contraseña debe tener una longitud mínima de 8 carácteres.");
			}

			if (repoUsuario.GetAll().Any(x => x.Nombre == dto.Nombre))
			{
				errores.Add("Ya existe un usuario con el mismo nombre.");
			}
			//generar expresion regular para la contraseña tenga por lo menos una mayiuscula y una misnuscula
			if (!Regex.IsMatch(dto.Contrasena, @"^(?=.*[a-zñ])(?=.*[A-ZÑ]).*$"))
			{
				errores.Add("La contraseña debe tener al menos una mayúscula y una minúscula.");
			}
			return !errores.Any();
		}

		public bool ValidateRegister(RegistrarUsuarioDTO dto, out List<string> errores)
		{
			errores = new List<string>();
			if (string.IsNullOrWhiteSpace(dto.Nombre))
			{
				errores.Add("El nombre de usuario está vacío.");
			}
			else if (dto.Nombre.Length > 45)
			{
				errores.Add("El nombre de usuario debe tener una longitud máxima de 45 carácteres.");
			}

			if (string.IsNullOrWhiteSpace(dto.Contrasena))
			{
				errores.Add("La contraseña está vacía.");
			}
			if (dto.Contrasena.Length > 8)
			{
				errores.Add("La contraseña debe tener una longitud máxima de 8 carácteres.");
			}

			if (repoUsuario.GetAll().Any(x => x.Nombre == dto.Nombre))
			{
				errores.Add("Ya existe un usuario con el mismo nombre.");
			}
			//generar expresion regular para la contraseña tenga por lo menos una mayiuscula y una misnuscula
			if (!Regex.IsMatch(dto.Contrasena, @"^(?=.*[a-zñ])(?=.*[A-ZÑ]).*$"))
			{
				errores.Add("La contraseña debe tener al menos una mayúscula y una minúscula.");
			}
			return !errores.Any();
		}
	}
}
