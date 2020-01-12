using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Brash.Infrastructure;
using LoyalGuard.Domain.Model;
using LoyalGuard.Infrastructure.Sqlite.Service;

namespace LoyalGuard.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class LGRightController : ControllerBase
	{
		private LGRightService _lGRightService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGRightController(LGRightService lGRightService, Serilog.ILogger logger) : base()
		{
			_lGRightService = lGRightService;
			_logger = logger;
		}
		
		// GET /api/LGRight/
		[HttpGet]
		public ActionResult<IEnumerable<LGRight>> Get()
		{
			var queryResult = _lGRightService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGRight/5
		[HttpGet("{id}")]
		public ActionResult<LGRight> Get(int id)
		{
			var model = new LGRight()
			{
				LGRightId = id
			};
		
			var serviceResult = _lGRightService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGRight
		[HttpPost]
		public ActionResult<LGRight> Post([FromBody] LGRight model)
		{
			var serviceResult = _lGRightService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGRight/6
		[HttpPut("{id}")]
		public ActionResult<LGRight> Put(int id, [FromBody] LGRight model)
		{
			model.LGRightId = id;
			
			var serviceResult = _lGRightService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGRight/6
		[HttpDelete("{id}")]
		public ActionResult<LGRight> Delete(int id)
		{
			var model = new LGRight()
			{
				LGRightId = id
			};
		
			var serviceResult = _lGRightService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}