using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;
using EncuestasAPI.Repositories;
using System.Text.RegularExpressions;

namespace EncuestasAPI.Models.Validators
{
	public class EncuestaValidator
	{
		private readonly Repository<Encuestas> repoEncuesta;

		public EncuestaValidator(Repository<Encuestas> repos)
		{
			this.repoEncuesta = repos;
		}

		public bool Validate(CrearEncuestaDTO dto, out List<string> errores)
		{
			errores = new List<string>();
			if (string.IsNullOrWhiteSpace(dto.Titulo))
			{
				errores.Add("El titulo está vacío.");
			}
			else if (dto.Titulo.Length > 150)
			{
				errores.Add("El nombre de usuario debe tener una longitud máxima de 150 carácteres.");
			}
			else if (dto.Preguntas.Count > 10)
			{
				errores.Add("Las preguntas deben de ser una cantidad máxima de 10 preguntas por encuesta.");
			}


			if (repoEncuesta.GetAll().Any(x => x.Titulo == dto.Titulo))
			{
				errores.Add("Ya existe una encuesta con el mismo titulo.");
			}
		
			return !errores.Any();
		}
	}
}
