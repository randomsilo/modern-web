
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGRightService 
	{
		BrashActionResult<LGRight> Create(LGRight model);
		BrashActionResult<LGRight> Fetch(LGRight model);
		BrashActionResult<LGRight> Update(LGRight model);
		BrashActionResult<LGRight> Delete(LGRight model);
		BrashQueryResult<LGRight> FindWhere(string where);
	}
}