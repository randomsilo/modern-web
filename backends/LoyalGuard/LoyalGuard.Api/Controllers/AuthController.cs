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
    private AuthService _authService { get; set; }
    
		private Serilog.ILogger _logger { get; set; }
		
		public AuthController(AuthService authService, Serilog.ILogger logger) : base()
		{
      _authService = authService;
			_logger = logger;
		}
		
		// POST api/Auth
		[HttpPost]
		public ActionResult<AccountAccess> Post([FromBody] AccountSignin accountSignin)
		{
      var result = _authService.Authenticate(accountSignin);

      if (result.Status == BrashActionStatus.ERROR || result.Status == BrashActionStatus.UNKNOWN)
        return StatusCode(500);

      if (result.Status == BrashActionStatus.NOT_FOUND)
        return StatusCode(404);

      if (result.Model == null)
        return StatusCode(404);

			return result.Model;
		}
		
		
		
	}
}