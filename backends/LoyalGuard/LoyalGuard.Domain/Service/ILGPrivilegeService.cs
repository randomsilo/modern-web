
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Domain.Service
{
	public interface ILGPrivilegeService 
	{
		BrashActionResult<LGPrivilege> Create(LGPrivilege model);
		BrashActionResult<LGPrivilege> Fetch(LGPrivilege model);
		BrashActionResult<LGPrivilege> Update(LGPrivilege model);
		BrashActionResult<LGPrivilege> Delete(LGPrivilege model);
		BrashQueryResult<LGPrivilege> FindWhere(string where);
		BrashQueryResult<LGPrivilege> FindByParent(int id);
	}
}