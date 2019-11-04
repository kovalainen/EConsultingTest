using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleClientApp
{
	public class Interval
	{
		public int Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
