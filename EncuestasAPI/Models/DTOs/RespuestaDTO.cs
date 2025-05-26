namespace EncuestasAPI.Models.DTOs
{
	public class RespuestaDTO
	{
		public int Id { get; set; }
		public int IdEncuesta { get; set; }
		public int IdUsuarioAplicador { get; set; }
		public string NombreAlumno { get; set; } = null!;
		public string NumControlAlumno { get; set; } = null!;
		public DateTime FechaAplicacion { get; set; }

	}
}
