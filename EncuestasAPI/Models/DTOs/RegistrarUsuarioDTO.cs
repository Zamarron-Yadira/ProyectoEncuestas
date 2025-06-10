namespace EncuestasAPI.Models.DTOs
{
	public class RegistrarUsuarioDTO
	{
		public string Nombre { get; set; } = null!;
		public string Contrasena { get; set; } = null!;

		public DateTime FechaRegistro { get; set; }
		public string EsAdmin { get; set; } = "";
	}
}
