namespace EncuestasAPI.Models.DTOs
{
	public class RegistrarUsuarioDTO
	{
		public string Nombre { get; set; } = null!;
		public string Contrasena { get; set; } = null!;

		public DateTime FechaRegistro { get; set; }
		public int EsAdmin { get; set; }// 2 = No, 1 = Sí
	}
}
