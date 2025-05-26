namespace EncuestasAPI.Models.DTOs
{
	public class PreguntaDTO
	{
		public int Id { get; set; }
		public string Descripcion { get; set; } = null!;
		public int IdEncuesta { get; set; }
		public int NumeroPregunta { get; set; }

	}
}
