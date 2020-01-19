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
	public class AuthController : ControllerBase
	{
    private LGAccountService _lGAccountService { get; set; }
		private LGTokenService _lGTokenService { get; set; }
    
		private Serilog.ILogger _logger { get; set; }
		
		public AuthController(LGAccountService lGAccountService, LGTokenService lGTokenService, Serilog.ILogger logger) : base()
		{
      _lGAccountService = lGAccountService;
			_lGTokenService = lGTokenService;
			_logger = logger;
		}
		
		// POST api/Auth
		[HttpPost]
		public ActionResult<LGToken> Post([FromBody] LGAccount incomingAccount)
		{
			var token = new LGToken();
			var accountSearchResults = _lGAccountService.FindWhere($"WHERE UserName = '{incomingAccount.UserName}' ");

      if (accountSearchResults.Models.Count == 0)
        return NotFound();


			return token;
		}
		
		
		
	}
}