
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGFeatureService 
	{
		BrashActionResult<LGFeature> Create(LGFeature model);
		BrashActionResult<LGFeature> Fetch(LGFeature model);
		BrashActionResult<LGFeature> Update(LGFeature model);
		BrashActionResult<LGFeature> Delete(LGFeature model);
		BrashQueryResult<LGFeature> FindWhere(string where);
	}
}