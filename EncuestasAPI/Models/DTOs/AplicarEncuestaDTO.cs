namespace EncuestasAPI.Models.DTOs
{
	public class AplicarEncuestaDTO
	{
		public int IdEncuesta { get; set; }
		public int IdUsuarioAplicador { get; set; }
		public string NombreAlumno { get; set; } = null!;
		public string NumControlAlumno { get; set; } = null!;
		public List<RespuestaPreguntaDto> Respuestas { get; set; } = new();
	}
	public class RespuestaPreguntaDto
	{
		public int PreguntaId { get; set; }
		public int Valor { get; set; } // de 1 a 5
	}
}
