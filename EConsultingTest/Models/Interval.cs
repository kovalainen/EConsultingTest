using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConsultingTest.Models
{
	public class Interval : IEntity
	{
		public int Id { get; set; }
		[Column(TypeName = "Date")]
		public DateTime Start { get; set; }
		[Column(TypeName = "Date")]
		public DateTime End { get; set; }
	}
}
