
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGAccountService 
	{
		BrashActionResult<LGAccount> Create(LGAccount model);
		BrashActionResult<LGAccount> Fetch(LGAccount model);
		BrashActionResult<LGAccount> Update(LGAccount model);
		BrashActionResult<LGAccount> Delete(LGAccount model);
		BrashQueryResult<LGAccount> FindWhere(string where);
	}
}