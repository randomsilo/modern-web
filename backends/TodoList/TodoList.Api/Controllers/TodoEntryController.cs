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
	public class TodoEntryController : ControllerBase
	{
		private TodoEntryService _todoEntryService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public TodoEntryController(TodoEntryService todoEntryService, Serilog.ILogger logger) : base()
		{
			_todoEntryService = todoEntryService;
			_logger = logger;
		}
		
		// GET /api/TodoEntry/
		[HttpGet]
		public ActionResult<IEnumerable<TodoEntry>> Get()
		{
			var queryResult = _todoEntryService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/TodoEntry/5
		[HttpGet("{id}")]
		public ActionResult<TodoEntry> Get(int id)
		{
			var model = new TodoEntry()
			{
				TodoEntryId = id
			};
		
			var serviceResult = _todoEntryService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/TodoEntry
		[HttpPost]
		public ActionResult<TodoEntry> Post([FromBody] TodoEntry model)
		{
			var serviceResult = _todoEntryService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/TodoEntry/6
		[HttpPut("{id}")]
		public ActionResult<TodoEntry> Put(int id, [FromBody] TodoEntry model)
		{
			model.TodoEntryId = id;
			
			var serviceResult = _todoEntryService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/TodoEntry/6
		[HttpDelete("{id}")]
		public ActionResult<TodoEntry> Delete(int id)
		{
			var model = new TodoEntry()
			{
				TodoEntryId = id
			};
		
			var serviceResult = _todoEntryService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}