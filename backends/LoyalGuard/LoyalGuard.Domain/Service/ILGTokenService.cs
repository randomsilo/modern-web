
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGTokenService 
	{
		BrashActionResult<LGToken> Create(LGToken model);
		BrashActionResult<LGToken> Fetch(LGToken model);
		BrashActionResult<LGToken> Update(LGToken model);
		BrashActionResult<LGToken> Delete(LGToken model);
		BrashQueryResult<LGToken> FindWhere(string where);
		BrashQueryResult<LGToken> FindByParent(int id);
	}
}