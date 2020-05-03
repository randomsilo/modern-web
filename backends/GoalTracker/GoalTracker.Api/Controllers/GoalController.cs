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
	public class GoalController : ControllerBase
	{
		private GoalService _goalService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public GoalController(GoalService goalService, Serilog.ILogger logger) : base()
		{
			_goalService = goalService;
			_logger = logger;
		}
		
		// GET /api/Goal/
		[HttpGet]
		public ActionResult<IEnumerable<Goal>> Get()
		{
			var queryResult = _goalService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/Goal/5
		[HttpGet("{id}")]
		public ActionResult<Goal> Get(int id)
		{
			var model = new Goal()
			{
				GoalId = id
			};
		
			var serviceResult = _goalService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/Goal
		[HttpPost]
		public ActionResult<Goal> Post([FromBody] Goal model)
		{
			var serviceResult = _goalService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/Goal/6
		[HttpPut("{id}")]
		public ActionResult<Goal> Put(int id, [FromBody] Goal model)
		{
			model.GoalId = id;
			
			var serviceResult = _goalService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/Goal/6
		[HttpDelete("{id}")]
		public ActionResult<Goal> Delete(int id)
		{
			var model = new Goal()
			{
				GoalId = id
			};
		
			var serviceResult = _goalService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}