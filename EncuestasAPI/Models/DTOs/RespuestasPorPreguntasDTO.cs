namespace EncuestasAPI.Models.DTOs
{
	public class RespuestasPorPreguntasDTO
	{
		public int IdPregunta { get; set; }
		public string Descripcion { get; set; } = null!;
		public int CantidadRespuestas { get; set; }
	}
}
