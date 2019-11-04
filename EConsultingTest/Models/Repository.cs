using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EConsultingTest.Models
{
	public class Repository<T> : IRepository<T> where T : class, IEntity
	{
		private readonly DatabaseContext m_databaseContext;
		public Repository(DatabaseContext databaseContext) =>
			m_databaseContext = databaseContext;
		public async Task Create(T entity)
		{
			await m_databaseContext.Set<T>().AddAsync(entity);
			await m_databaseContext.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var entity = await GetById(id);
			m_databaseContext.Set<T>().Remove(entity);
			await m_databaseContext.SaveChangesAsync();
		}

		public IQueryable<T> GetAll() => m_databaseContext.Set<T>();

		public async Task<T> GetById(int id) =>
			await m_databaseContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

		public async Task Update(T entity)
		{
			m_databaseContext.Set<T>().Update(entity);
			await m_databaseContext.SaveChangesAsync();
		}
	}
}
