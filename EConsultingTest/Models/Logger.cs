using System;
using System.Threading.Tasks;

namespace EConsultingTest.Models
{
	public class Logger
	{
		private readonly IRepository<Log> m_repository;
		public Logger(IRepository<Log> repository)
		{
			m_repository = repository;
		}
		public async Task Succes(string method, string message)
		{
			await m_repository.Create(new Log()
			{
				Action = method,
				DateTime = DateTime.Now,
				Status = "200",
				Message = message,
			});
		}
		public async Task Error(string method, string message)
		{
			await m_repository.Create(new Log()
			{
				Action = method,
				DateTime = DateTime.Now,
				Status = "400",
				Message = message,
			});
		}
	}
}
