using EncuestasAPI.Models.Entities;
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
