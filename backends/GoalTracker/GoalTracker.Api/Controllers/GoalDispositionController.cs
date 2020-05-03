using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Brash.Infrastructure;
using GoalTracker.Domain.Model;
using GoalTracker.Infrastructure.Sqlite.Service;

namespace GoalTracker.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class GoalDispositionController : ControllerBase
	{
		private GoalDispositionService _goalDispositionService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public GoalDispositionController(GoalDispositionService goalDispositionService, Serilog.ILogger logger) : base()
		{
			_goalDispositionService = goalDispositionService;
			_logger = logger;
		}
		
		// GET /api/GoalDisposition/
		[HttpGet]
		public ActionResult<IEnumerable<GoalDisposition>> Get()
		{
			var queryResult = _goalDispositionService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/GoalDisposition/5
		[HttpGet("{id}")]
		public ActionResult<GoalDisposition> Get(int id)
		{
			var model = new GoalDisposition()
			{
				GoalDispositionId = id
			};
		
			var serviceResult = _goalDispositionService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/GoalDisposition
		[HttpPost]
		public ActionResult<GoalDisposition> Post([FromBody] GoalDisposition model)
		{
			var serviceResult = _goalDispositionService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/GoalDisposition/6
		[HttpPut("{id}")]
		public ActionResult<GoalDisposition> Put(int id, [FromBody] GoalDisposition model)
		{
			model.GoalDispositionId = id;
			
			var serviceResult = _goalDispositionService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/GoalDisposition/6
		[HttpDelete("{id}")]
		public ActionResult<GoalDisposition> Delete(int id)
		{
			var model = new GoalDisposition()
			{
				GoalDispositionId = id
			};
		
			var serviceResult = _goalDispositionService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}