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
	public class LGTokenController : ControllerBase
	{
		private LGTokenService _lGTokenService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGTokenController(LGTokenService lGTokenService, Serilog.ILogger logger) : base()
		{
			_lGTokenService = lGTokenService;
			_logger = logger;
		}
		
		// GET /api/LGToken/
		[HttpGet]
		public ActionResult<IEnumerable<LGToken>> Get()
		{
			var queryResult = _lGTokenService.FindWhere("WHERE 1 = 1 ORDER BY 1 ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGToken/5
		[HttpGet("{id}")]
		public ActionResult<LGToken> Get(int id)
		{
			var model = new LGToken()
			{
				LGTokenId = id
			};
		
			var serviceResult = _lGTokenService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGToken
		[HttpPost]
		public ActionResult<LGToken> Post([FromBody] LGToken model)
		{
			var serviceResult = _lGTokenService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGToken/6
		[HttpPut("{id}")]
		public ActionResult<LGToken> Put(int id, [FromBody] LGToken model)
		{
			model.LGTokenId = id;
			
			var serviceResult = _lGTokenService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGToken/6
		[HttpDelete("{id}")]
		public ActionResult<LGToken> Delete(int id)
		{
			var model = new LGToken()
			{
				LGTokenId = id
			};
		
			var serviceResult = _lGTokenService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}

		// GET /api/LGTokenByParent/4
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<LGToken>> GetByParent(int id)
		{
			var queryResult = _lGTokenService.FindByParent(id);
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
	}
}