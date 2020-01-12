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
	public class LGPrivilegeController : ControllerBase
	{
		private LGPrivilegeService _lGPrivilegeService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGPrivilegeController(LGPrivilegeService lGPrivilegeService, Serilog.ILogger logger) : base()
		{
			_lGPrivilegeService = lGPrivilegeService;
			_logger = logger;
		}
		
		// GET /api/LGPrivilege/
		[HttpGet]
		public ActionResult<IEnumerable<LGPrivilege>> Get()
		{
			var queryResult = _lGPrivilegeService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGPrivilege/5
		[HttpGet("{id}")]
		public ActionResult<LGPrivilege> Get(int id)
		{
			var model = new LGPrivilege()
			{
				LGPrivilegeId = id
			};
		
			var serviceResult = _lGPrivilegeService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGPrivilege
		[HttpPost]
		public ActionResult<LGPrivilege> Post([FromBody] LGPrivilege model)
		{
			var serviceResult = _lGPrivilegeService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGPrivilege/6
		[HttpPut("{id}")]
		public ActionResult<LGPrivilege> Put(int id, [FromBody] LGPrivilege model)
		{
			model.LGPrivilegeId = id;
			
			var serviceResult = _lGPrivilegeService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGPrivilege/6
		[HttpDelete("{id}")]
		public ActionResult<LGPrivilege> Delete(int id)
		{
			var model = new LGPrivilege()
			{
				LGPrivilegeId = id
			};
		
			var serviceResult = _lGPrivilegeService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}

		// GET /api/LGPrivilegeByParent/4
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<LGPrivilege>> GetByParent(int id)
		{
			var queryResult = _lGPrivilegeService.FindByParent(id);
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
	}
}