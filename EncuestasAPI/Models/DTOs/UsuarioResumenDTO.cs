namespace EncuestasAPI.Models.DTOs
{
	public class UsuarioResumenDTO
	{
		public string Nombre { get; set; } = null!;
		public DateTime FechaRegistro { get; set; }
		public bool EsAdmin { get; set; }
	}
}
