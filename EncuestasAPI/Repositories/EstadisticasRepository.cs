using EncuestasAPI.Models.DTOs;
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

		public int GetTotalEncuestasRespondidas()
		{
			return Context.Respuestas.Count();
		}

		public double GetPromedioRespuestasPorEncuesta()
		{
			var totalEncuestas = GetTotalEncuestasCreadas();
			if (totalEncuestas == 0) return 0;

			var totalRespuestas = GetTotalEncuestasRespondidas();
			return (double)totalRespuestas / totalEncuestas;
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

	}
}
