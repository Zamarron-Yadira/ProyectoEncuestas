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
			
			if (dto.Preguntas.Count > 10)
			{
				errores.Add("Las preguntas deben de ser una cantidad máxima de 10 preguntas por encuesta.");
			}


			if (repoEncuesta.GetAll().Any(x => x.Titulo == dto.Titulo))
			{
				errores.Add("Ya existe una encuesta con el mismo titulo.");
			}
		
			return !errores.Any();
		}

		public bool ValidateEditarEncuesta(EditarEncuestaDTO dto, out List<string> errores)
		{
			errores = new List<string>();

			if (string.IsNullOrWhiteSpace(dto.Titulo))
				errores.Add("El título está vacío.");

			if (dto.Titulo.Length > 150)
				errores.Add("El título no debe superar los 150 caracteres.");

			if (dto.Preguntas.Count > 10)
				errores.Add("Máximo 10 preguntas por encuesta.");

			var descripcionesDuplicadas = dto.Preguntas
				.GroupBy(p => p.Descripcion.Trim().ToLower())
				.Where(g => g.Count() > 1)
				.Select(g => g.Key).ToList();

			if (descripcionesDuplicadas.Any())
				errores.Add("Hay preguntas con descripciones duplicadas.");

			var numeros = dto.Preguntas.Select(p => p.NumeroPregunta).ToList();
			if (numeros.Any(n => n < 1 || n > 10))
				errores.Add("Los números de pregunta deben estar entre 1 y 10.");

			if (numeros.Distinct().Count() != numeros.Count)
				errores.Add("Hay números de pregunta duplicados.");

			if (!Enumerable.Range(1, dto.Preguntas.Count).All(n => numeros.Contains(n)))
				errores.Add("Los números de pregunta deben ser secuenciales del 1 al " + dto.Preguntas.Count);

			return !errores.Any();
		}



	}
}
