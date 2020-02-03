
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGAbilityService 
	{
		BrashActionResult<LGAbility> Create(LGAbility model);
		BrashActionResult<LGAbility> Fetch(LGAbility model);
		BrashActionResult<LGAbility> Update(LGAbility model);
		BrashActionResult<LGAbility> Delete(LGAbility model);
		BrashQueryResult<LGAbility> FindWhere(string where);
	}
}