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
	public class ToolsRequiredController : ControllerBase
	{
		private ToolsRequiredService _toolsRequiredService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public ToolsRequiredController(ToolsRequiredService toolsRequiredService, Serilog.ILogger logger) : base()
		{
			_toolsRequiredService = toolsRequiredService;
			_logger = logger;
		}
		
		// GET /api/ToolsRequired/
		[HttpGet]
		public ActionResult<IEnumerable<ToolsRequired>> Get()
		{
			var queryResult = _toolsRequiredService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/ToolsRequired/5
		[HttpGet("{id}")]
		public ActionResult<ToolsRequired> Get(int id)
		{
			var model = new ToolsRequired()
			{
				ToolsRequiredId = id
			};
		
			var serviceResult = _toolsRequiredService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/ToolsRequired
		[HttpPost]
		public ActionResult<ToolsRequired> Post([FromBody] ToolsRequired model)
		{
			var serviceResult = _toolsRequiredService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/ToolsRequired/6
		[HttpPut("{id}")]
		public ActionResult<ToolsRequired> Put(int id, [FromBody] ToolsRequired model)
		{
			model.ToolsRequiredId = id;
			
			var serviceResult = _toolsRequiredService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/ToolsRequired/6
		[HttpDelete("{id}")]
		public ActionResult<ToolsRequired> Delete(int id)
		{
			var model = new ToolsRequired()
			{
				ToolsRequiredId = id
			};
		
			var serviceResult = _toolsRequiredService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}

		// GET /api/ToolsRequiredByParent/4
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<ToolsRequired>> GetByParent(int id)
		{
			var queryResult = _toolsRequiredService.FindByParent(id);
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
	}
}