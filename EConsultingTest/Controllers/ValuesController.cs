using EConsultingTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EConsultingTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly IRepository<Interval> m_intervalRepository;
		private readonly Logger m_logger;

		public ValuesController(IRepository<Interval> intervalRepository, Logger logger)
		{
			m_intervalRepository = intervalRepository;
			m_logger = logger;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Interval>> Get()
		{
			return m_intervalRepository.GetAll().ToList();
		}

		[Authorize]
		[HttpPost("addinterval")]
		public async Task<ActionResult> AddInterval(Interval interval)
		{
			if (!CheckInterval(interval))
			{
				await m_logger.Error("AddInterval", "ERROR: The start date is less than the end date");

				return BadRequest("The start date is less than the end date");
			}
			await m_intervalRepository.Create(interval);
			await m_logger.Succes("AddInterval", "OK");

			return Ok();
		}

		[HttpPost("getintervalslist")]
		public async Task<ActionResult<IEnumerable<Interval>>> GetIntervalsList(Interval interval)
		{
			if (!CheckInterval(interval))
			{
				await m_logger.Error("GetIntervalsList", "ERROR: The start date is less than the end date");

				return BadRequest("The start date is less than the end date");
			}

			List<Interval> result = m_intervalRepository.GetAll().Where(i => i.Start >= interval.Start && i.End <= interval.End).ToList();
			await m_logger.Succes("GetIntervalsList", "OK");

			return result;
		}

		private bool CheckInterval(Interval interval)
		{
			if (interval == null)
			{
				return false;
			}

			return interval.Start <= interval.End;
		}

	}
}
