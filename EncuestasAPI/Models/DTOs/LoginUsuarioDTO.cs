namespace EncuestasAPI.Models.DTOs
{
	public class LoginUsuarioDTO
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public string Contrasena { get; set; } = null!;
		public int EsAdmin { get; set; }// 2 = No, 1 = Sí

	}
}
