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
	public class LGAccountController : ControllerBase
	{
		private LGAccountService _lGAccountService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGAccountController(LGAccountService lGAccountService, Serilog.ILogger logger) : base()
		{
			_lGAccountService = lGAccountService;
			_logger = logger;
		}
		
		// GET /api/LGAccount/
		[HttpGet]
		public ActionResult<IEnumerable<LGAccount>> Get()
		{
			var queryResult = _lGAccountService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGAccount/5
		[HttpGet("{id}")]
		public ActionResult<LGAccount> Get(int id)
		{
			var model = new LGAccount()
			{
				LGAccountId = id
			};
		
			var serviceResult = _lGAccountService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGAccount
		[HttpPost]
		public ActionResult<LGAccount> Post([FromBody] LGAccount model)
		{
			var serviceResult = _lGAccountService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGAccount/6
		[HttpPut("{id}")]
		public ActionResult<LGAccount> Put(int id, [FromBody] LGAccount model)
		{
			model.LGAccountId = id;
			
			var serviceResult = _lGAccountService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGAccount/6
		[HttpDelete("{id}")]
		public ActionResult<LGAccount> Delete(int id)
		{
			var model = new LGAccount()
			{
				LGAccountId = id
			};
		
			var serviceResult = _lGAccountService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}