using AutoMapper;
using EncuestasAPI.Models.DTOs;
using EncuestasAPI.Models.Entities;

namespace EncuestasAPI.Helpers
{
	public class AutomapperProfile:Profile
	{
		public AutomapperProfile()
		{
			CreateMap<Usuarios, LoginUsuarioDTO>().ReverseMap();


			//Encuestas
			CreateMap<Encuestas, EncuestaDTO>();
			CreateMap<CrearEncuestaDTO, Encuestas>();
			CreateMap<CrearPreguntaDto, Preguntas>();

			//Preguntas
			CreateMap<Preguntas, PreguntaDTO>();

			//Respuestas
			CreateMap<Respuestas, RespuestaDTO>();
			CreateMap<AplicarEncuestaDTO, Respuestas>();
			CreateMap<RespuestaPreguntaDto, Detallerespuestas>();

			//Detallerespuestas
			CreateMap<Detallerespuestas, RespuestaDetalleDTO>();
		}
	}
}
