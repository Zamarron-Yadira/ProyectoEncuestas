namespace EncuestasAPI.Models.DTOs
{
	public class PreguntasConRespuestaPorAlumnoDTO
	{

		public int IdEncuesta { get; set; }
		public int IdAlumno { get; set; }
		public string Nombre { get; set; } = null!;
		public string NumeroControl { get; set; } = null!;

		public List<PreguntaConRespuestaDTO> Preguntas { get; set; } = new();


		public class PreguntaConRespuestaDTO
		{
			public int IdPregunta { get; set; }
			public string Descripcion { get; set; } = null!;

			//public int IdRespuesta { get; set; }
			public int ValorRespuesta { get; set; }
		}

	}
}
