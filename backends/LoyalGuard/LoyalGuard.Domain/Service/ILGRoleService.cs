
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGRoleService 
	{
		BrashActionResult<LGRole> Create(LGRole model);
		BrashActionResult<LGRole> Fetch(LGRole model);
		BrashActionResult<LGRole> Update(LGRole model);
		BrashActionResult<LGRole> Delete(LGRole model);
		BrashQueryResult<LGRole> FindWhere(string where);
	}
}