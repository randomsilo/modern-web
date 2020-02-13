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
	public class TodoNotesController : ControllerBase
	{
		private TodoNotesService _todoNotesService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public TodoNotesController(TodoNotesService todoNotesService, Serilog.ILogger logger) : base()
		{
			_todoNotesService = todoNotesService;
			_logger = logger;
		}
		
		// GET /api/TodoNotes/
		[HttpGet]
		public ActionResult<IEnumerable<TodoNotes>> Get()
		{
			var queryResult = _todoNotesService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/TodoNotes/5
		[HttpGet("{id}")]
		public ActionResult<TodoNotes> Get(int id)
		{
			var model = new TodoNotes()
			{
				TodoNotesId = id
			};
		
			var serviceResult = _todoNotesService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/TodoNotes
		[HttpPost]
		public ActionResult<TodoNotes> Post([FromBody] TodoNotes model)
		{
			var serviceResult = _todoNotesService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/TodoNotes/6
		[HttpPut("{id}")]
		public ActionResult<TodoNotes> Put(int id, [FromBody] TodoNotes model)
		{
			model.TodoNotesId = id;
			
			var serviceResult = _todoNotesService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/TodoNotes/6
		[HttpDelete("{id}")]
		public ActionResult<TodoNotes> Delete(int id)
		{
			var model = new TodoNotes()
			{
				TodoNotesId = id
			};
		
			var serviceResult = _todoNotesService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}

		// GET /api/TodoNotesByParent/4
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<TodoNotes>> GetByParent(int id)
		{
			var queryResult = _todoNotesService.FindByParent(id);
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
	}
}