using System.Linq;
using System.Threading.Tasks;

namespace EConsultingTest.Models
{
	public interface IRepository<T> where T : class, IEntity
	{
		IQueryable<T> GetAll();
		Task<T> GetById(int id);
		Task Create(T entity);
		Task Update(T entity);
		Task Delete(int id);
	}
}
