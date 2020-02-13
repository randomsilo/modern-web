using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Brash.Infrastructure;
using TodoList.Domain.Model;
using TodoList.Infrastructure.Sqlite.Service;

namespace TodoList.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TodoStatusController : ControllerBase
	{
		private TodoStatusService _todoStatusService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public TodoStatusController(TodoStatusService todoStatusService, Serilog.ILogger logger) : base()
		{
			_todoStatusService = todoStatusService;
			_logger = logger;
		}
		
		// GET /api/TodoStatus/
		[HttpGet]
		public ActionResult<IEnumerable<TodoStatus>> Get()
		{
			var queryResult = _todoStatusService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/TodoStatus/5
		[HttpGet("{id}")]
		public ActionResult<TodoStatus> Get(int id)
		{
			var model = new TodoStatus()
			{
				TodoStatusId = id
			};
		
			var serviceResult = _todoStatusService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/TodoStatus
		[HttpPost]
		public ActionResult<TodoStatus> Post([FromBody] TodoStatus model)
		{
			var serviceResult = _todoStatusService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/TodoStatus/6
		[HttpPut("{id}")]
		public ActionResult<TodoStatus> Put(int id, [FromBody] TodoStatus model)
		{
			model.TodoStatusId = id;
			
			var serviceResult = _todoStatusService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/TodoStatus/6
		[HttpDelete("{id}")]
		public ActionResult<TodoStatus> Delete(int id)
		{
			var model = new TodoStatus()
			{
				TodoStatusId = id
			};
		
			var serviceResult = _todoStatusService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}