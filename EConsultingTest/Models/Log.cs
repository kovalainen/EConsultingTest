using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EConsultingTest.Models
{
	public class Log : IEntity
	{
		public int Id { get; set; }
		public DateTime DateTime { get; set; }
		public string Action { get; set; }
		public string Status { get; set; }
		public string Message { get; set; }
	}
}
