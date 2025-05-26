namespace EncuestasAPI.Models.DTOs
{
	public class CrearEncuestaDTO
	{
		public int IdUsuario { get; set; }
		public string Titulo { get; set; } = null!;
		public List<CrearPreguntaDto> Preguntas { get; set; } = new();
	}
	public class CrearPreguntaDto
	{
		public string Descripcion { get; set; } = null!; 
		public int NumeroPregunta { get; set; } // del 1 al 10
	}
}
