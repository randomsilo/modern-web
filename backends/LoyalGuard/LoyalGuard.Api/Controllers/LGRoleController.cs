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
	public class LGRoleController : ControllerBase
	{
		private LGRoleService _lGRoleService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGRoleController(LGRoleService lGRoleService, Serilog.ILogger logger) : base()
		{
			_lGRoleService = lGRoleService;
			_logger = logger;
		}
		
		// GET /api/LGRole/
		[HttpGet]
		public ActionResult<IEnumerable<LGRole>> Get()
		{
			var queryResult = _lGRoleService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGRole/5
		[HttpGet("{id}")]
		public ActionResult<LGRole> Get(int id)
		{
			var model = new LGRole()
			{
				LGRoleId = id
			};
		
			var serviceResult = _lGRoleService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGRole
		[HttpPost]
		public ActionResult<LGRole> Post([FromBody] LGRole model)
		{
			var serviceResult = _lGRoleService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGRole/6
		[HttpPut("{id}")]
		public ActionResult<LGRole> Put(int id, [FromBody] LGRole model)
		{
			model.LGRoleId = id;
			
			var serviceResult = _lGRoleService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGRole/6
		[HttpDelete("{id}")]
		public ActionResult<LGRole> Delete(int id)
		{
			var model = new LGRole()
			{
				LGRoleId = id
			};
		
			var serviceResult = _lGRoleService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}