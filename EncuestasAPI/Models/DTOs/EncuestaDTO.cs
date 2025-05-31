namespace EncuestasAPI.Models.DTOs
{
	public class EncuestaDTO
	{
		public int Id { get; set; }
		public int IdUsuario { get; set; }
		public string Titulo { get; set; } = null!;
		public DateTime FechaCreacion { get; set; }
		public List<PreguntasDTO> Preguntas { get; set; }
	}
	public class PreguntasDTO
	{
		public int Id { get; set; }
		public string Descripcion { get; set; }
		public int NumeroPregunta { get; set; }
	}
}
