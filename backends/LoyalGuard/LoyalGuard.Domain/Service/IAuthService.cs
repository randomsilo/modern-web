
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface IAuthService 
	{
		BrashActionResult<AccountAccess> Authenticate(AccountSignin model);
	}
}