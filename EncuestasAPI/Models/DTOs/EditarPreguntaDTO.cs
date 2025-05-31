namespace EncuestasAPI.Models.DTOs
{
	public class EditarPreguntaDTO
	{
		public int Id { get; set; }
		public string Descripcion { get; set; } = null!;
		public int NumeroPregunta { get; set; }
	}
}
