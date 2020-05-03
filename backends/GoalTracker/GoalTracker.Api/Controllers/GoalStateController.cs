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
	public class GoalStateController : ControllerBase
	{
		private GoalStateService _goalStateService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public GoalStateController(GoalStateService goalStateService, Serilog.ILogger logger) : base()
		{
			_goalStateService = goalStateService;
			_logger = logger;
		}
		
		// GET /api/GoalState/
		[HttpGet]
		public ActionResult<IEnumerable<GoalState>> Get()
		{
			var queryResult = _goalStateService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/GoalState/5
		[HttpGet("{id}")]
		public ActionResult<GoalState> Get(int id)
		{
			var model = new GoalState()
			{
				GoalStateId = id
			};
		
			var serviceResult = _goalStateService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/GoalState
		[HttpPost]
		public ActionResult<GoalState> Post([FromBody] GoalState model)
		{
			var serviceResult = _goalStateService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/GoalState/6
		[HttpPut("{id}")]
		public ActionResult<GoalState> Put(int id, [FromBody] GoalState model)
		{
			model.GoalStateId = id;
			
			var serviceResult = _goalStateService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/GoalState/6
		[HttpDelete("{id}")]
		public ActionResult<GoalState> Delete(int id)
		{
			var model = new GoalState()
			{
				GoalStateId = id
			};
		
			var serviceResult = _goalStateService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}