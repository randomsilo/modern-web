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
	public class GoalStatusController : ControllerBase
	{
		private GoalStatusService _goalStatusService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public GoalStatusController(GoalStatusService goalStatusService, Serilog.ILogger logger) : base()
		{
			_goalStatusService = goalStatusService;
			_logger = logger;
		}
		
		// GET /api/GoalStatus/
		[HttpGet]
		public ActionResult<IEnumerable<GoalStatus>> Get()
		{
			var queryResult = _goalStatusService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/GoalStatus/5
		[HttpGet("{id}")]
		public ActionResult<GoalStatus> Get(int id)
		{
			var model = new GoalStatus()
			{
				GoalStatusId = id
			};
		
			var serviceResult = _goalStatusService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/GoalStatus
		[HttpPost]
		public ActionResult<GoalStatus> Post([FromBody] GoalStatus model)
		{
			var serviceResult = _goalStatusService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/GoalStatus/6
		[HttpPut("{id}")]
		public ActionResult<GoalStatus> Put(int id, [FromBody] GoalStatus model)
		{
			model.GoalStatusId = id;
			
			var serviceResult = _goalStatusService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/GoalStatus/6
		[HttpDelete("{id}")]
		public ActionResult<GoalStatus> Delete(int id)
		{
			var model = new GoalStatus()
			{
				GoalStatusId = id
			};
		
			var serviceResult = _goalStatusService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}