using EncuestasAPI.Models.Entities;
using EncuestasAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EncuestasAPI.Repositories
{
	public class Repository<T> where T : class
	{
		public EncuestasContext Context { get; set; }

		public Repository(EncuestasContext context)
		{
			Context = context;
		}

		public IEnumerable<T> GetAll()
		{
			return Context.Set<T>();
		}

		public T? GetId(object id)
		{
			return Context.Find<T>(id);
		}

		public Encuestas? GetxId(int id)
		{
			return Context.Encuestas
			   .Include(e => e.Preguntas.OrderBy(x=>x.NumeroPregunta))
			   .FirstOrDefault(e => e.Id == id);
		}
		public EncuestaDTO? GetConPreguntas(int id)
		{
			return Context.Encuestas
				.Where(e => e.Id == id)
				.Select(e => new EncuestaDTO
				{
					Id = e.Id,
					Titulo = e.Titulo,
					Preguntas = e.Preguntas
						.OrderBy(p => p.NumeroPregunta)
						.Select(p => new PreguntasDTO
						{
							Id = p.IdPregunta,
							Descripcion = p.Descripcion,
							NumeroPregunta = p.NumeroPregunta
						}).ToList()
				}).FirstOrDefault();
		
		}
		//CRUD	
		public void Insert(T entity)
		{
			Context.Set<T>().Add(entity);
			Context.SaveChanges();
		}

		public void Update(T entity)
		{
			Context.Set<T>().Update(entity);
			Context.SaveChanges();
		}
		public void Delete(object id)
		{
			var entity = Context.Find<T>(id);
			if (entity != null)
			{
				Context.Remove(entity);
				Context.SaveChanges();
			}

		}
	

	}
}
