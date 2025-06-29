﻿using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;

namespace EncuestasAPI.Repositories
{
	public class EstadisticasRepository
	{
		public EncuestasContext Context { get; set; }


		public EstadisticasRepository(EncuestasContext context) {
			Context = context;
		}
		public int GetTotalEncuestasCreadas()
		{
			return Context.Encuestas.Count();
		}

		public int GetTotalEncuestasSinResponder()
		{
			return Context.Encuestas
				.Count(e => !Context.Respuestas.Any(r => r.IdEncuesta == e.Id));
		}


		public int GetTotalEncuestasRespondidas()
		{
			return Context.Respuestas.Count();
		}

		public double GetPromedioRespuestasPorEncuesta()
		{
			var totalEncuestas = GetTotalEncuestasCreadas();
			if (totalEncuestas == 0) return 0;

			var totalRespuestas = GetTotalEncuestasRespondidas();
			return Math.Round((double)totalRespuestas / totalEncuestas, 2);
		}
		public int GetTotalAlumnosEntrevistados()
		{
			return Context.Respuestas
				.Select(r => r.NumControlAlumno)
				.Distinct()
				.Count();
		}
		public List<EncuestaConTotalRespuestasDTO> GetEncuestasConTotalRespuestas()
		{
			var encuestas = Context.Encuestas
				.Select(e => new EncuestaConTotalRespuestasDTO
				{
					NombreEncuesta = e.Titulo,
					TotalRespuestas = Context.Respuestas.Count(r => r.IdEncuesta == e.Id)
				})
				.ToList();

			return encuestas;
		}

		//Preguntas
		public int GetTotalPreguntas()
		{
			return Context.Preguntas.Count();
		}
		public List<RespuestasPorPreguntasDTO> GetCantidadRespuestasPorPregunta()
		{
			var resultado = Context.Preguntas.Select(p => new RespuestasPorPreguntasDTO
			{
				IdPregunta = p.IdPregunta,
				Descripcion = p.Descripcion,
				CantidadRespuestas = p.Detallerespuestas.Count()
			})
				.ToList();
			return resultado;
		}


		//USUARIOS RESPONDIENDO:

	}
}
