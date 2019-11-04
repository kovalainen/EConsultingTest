using Microsoft.EntityFrameworkCore;

namespace EConsultingTest.Models
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts) { }
		public DbSet<Interval> Intervals { get; set; }
		public DbSet<Log> Logs { get; set; }
	}
}
