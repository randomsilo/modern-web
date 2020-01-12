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
	public class LGFeatureController : ControllerBase
	{
		private LGFeatureService _lGFeatureService { get; set; }
		private Serilog.ILogger _logger { get; set; }
		
		public LGFeatureController(LGFeatureService lGFeatureService, Serilog.ILogger logger) : base()
		{
			_lGFeatureService = lGFeatureService;
			_logger = logger;
		}
		
		// GET /api/LGFeature/
		[HttpGet]
		public ActionResult<IEnumerable<LGFeature>> Get()
		{
			var queryResult = _lGFeatureService.FindWhere("WHERE IFNULL(IsDisabled, 0) = 0 ORDER BY OrderNo ");
			if (queryResult.Status == BrashQueryStatus.ERROR)
				return BadRequest(queryResult.Message);
		
			return queryResult.Models;
		}
		
		// GET api/LGFeature/5
		[HttpGet("{id}")]
		public ActionResult<LGFeature> Get(int id)
		{
			var model = new LGFeature()
			{
				LGFeatureId = id
			};
		
			var serviceResult = _lGFeatureService.Fetch(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
		
			return serviceResult.Model;
		}
		
		// POST api/LGFeature
		[HttpPost]
		public ActionResult<LGFeature> Post([FromBody] LGFeature model)
		{
			var serviceResult = _lGFeatureService.Create(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// PUT api/LGFeature/6
		[HttpPut("{id}")]
		public ActionResult<LGFeature> Put(int id, [FromBody] LGFeature model)
		{
			model.LGFeatureId = id;
			
			var serviceResult = _lGFeatureService.Update(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
			if (serviceResult.Status == BrashActionStatus.NOT_FOUND)
				return NotFound(serviceResult.Message);
			
			return serviceResult.Model;
		}
		
		// DELETE api/LGFeature/6
		[HttpDelete("{id}")]
		public ActionResult<LGFeature> Delete(int id)
		{
			var model = new LGFeature()
			{
				LGFeatureId = id
			};
		
			var serviceResult = _lGFeatureService.Delete(model);
			if (serviceResult.Status == BrashActionStatus.ERROR)
				return BadRequest(serviceResult.Message);
		
			return serviceResult.Model;
		}
	}
}