namespace EncuestasAPI.Models.DTOs
{
	public class RespuestaDetalleDTO
	{
		public int Id { get; set; }
		public int IdRespuesta { get; set; }
		public int IdPregunta { get; set; }
		public int ValorEvaluacion { get; set; }
	}
}
