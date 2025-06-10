namespace EncuestasAPI.Models.DTOs
{
	public class UsuarioResumenDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public DateTime FechaRegistro { get; set; }
		public string EsAdmin { get; set; } = "";
	}
}
