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
	public class LGAbilityController : ControllerBase
	{
		private LGAbilityService _lGAbilityService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGAbilityController(LGAbilityService lGAbilityService, Serilog.ILogger logger) : base()
		{
			_lGAbilityService = lGAbilityService;
			_logger = logger;
		}
		
		// GET /api/LGAbility/
		[HttpGet]
		public ActionResult<IEnumerable<LGAbility>> Get()
		{
			var queryResult = _lGAbilityService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGAbility/5
		[HttpGet("{id}")]
		public ActionResult<LGAbility> Get(int id)
		{
			var model = new LGAbility()
			{
				LGAbilityId = id
			};
		
			var serviceResult = _lGAbilityService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGAbility
		[HttpPost]
		public ActionResult<LGAbility> Post([FromBody] LGAbility model)
		{
			var serviceResult = _lGAbilityService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGAbility/6
		[HttpPut("{id}")]
		public ActionResult<LGAbility> Put(int id, [FromBody] LGAbility model)
		{
			model.LGAbilityId = id;
			
			var serviceResult = _lGAbilityService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGAbility/6
		[HttpDelete("{id}")]
		public ActionResult<LGAbility> Delete(int id)
		{
			var model = new LGAbility()
			{
				LGAbilityId = id
			};
		
			var serviceResult = _lGAbilityService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}