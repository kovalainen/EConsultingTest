using System.Threading.Tasks;
using Xunit;
using EConsultingTest.Controllers;
using EConsultingTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace XUnitTests
{
	public class ControllerTests : IDisposable
	{
		private readonly DatabaseContext databaseContext;
		public ControllerTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
			var options = optionsBuilder
					.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EConsultingTest;")
					.Options;
			databaseContext = new DatabaseContext(options);
		}

		[Fact]
		public async Task SendNullToAddInterval()
		{
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			IActionResult result = await controller.AddInterval(null);
			Assert.IsType<BadRequestObjectResult>(result);
		}

		[Fact]
		public async Task SendIncorrectDatesToAddInterval()
		{
			Interval interval = new Interval()
			{
				Start = DateTime.Now,
				End = DateTime.Now.AddDays(-1),
			};
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			IActionResult result = await controller.AddInterval(interval);
			Assert.IsType<BadRequestObjectResult>(result);
		}

		[Fact]
		public async Task SendCorrectDatesToAddInterval()
		{
			Interval interval = new Interval()
			{
				Start = DateTime.Now,
				End = DateTime.Now.AddDays(23),
			};
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			IActionResult result = await controller.AddInterval(interval);
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task SendNullToGetIntervalsList()
		{
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			var result = await controller.GetIntervalsList(null);
			IEnumerable<Interval> intervals = result.Value;
			Assert.Null(intervals);
		}

		[Fact]
		public async Task SendIncorrectDatesToGetIntervalsList()
		{
			Interval interval = new Interval()
			{
				Start = DateTime.Now,
				End = DateTime.Now.AddDays(-5),
			};
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			var result = await controller.GetIntervalsList(interval);
			IEnumerable<Interval> intervals = result.Value;
			Assert.Null(intervals);
		}

		[Fact]
		public async Task SendCorrectDatesToGetIntervalsList()
		{
			Interval interval = new Interval()
			{
				Start = DateTime.Now,
				End = DateTime.Now.AddDays(30),
			};
			IRepository<Interval> repository = new Repository<Interval>(databaseContext);
			Logger logger = new Logger(new Repository<Log>(databaseContext));
			ValuesController controller = new ValuesController(repository, logger);
			var result = await controller.GetIntervalsList(interval);
			IEnumerable<Interval> intervals = result.Value;
			Assert.NotNull(intervals);
		}

		public void Dispose()
		{
			databaseContext?.Dispose();
		}
	}
}
