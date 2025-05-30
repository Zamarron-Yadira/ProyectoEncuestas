namespace EncuestasAPI.Models.DTOs
{
	public class EstadisticasAdminDTO
	{
		public int TotalEncuestasCreadas { get; set; }
		public int TotalEncuestasRespondidas { get; set; }
		public double PromedioRespuestasPorEncuesta { get; set; }
	}
}
