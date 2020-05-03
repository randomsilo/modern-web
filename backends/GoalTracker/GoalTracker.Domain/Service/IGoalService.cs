
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using GoalTracker.Domain.Model;

namespace GoalTracker.Domain.Service
{
	public interface IGoalService 
	{
		BrashActionResult<Goal> Create(Goal model);
		BrashActionResult<Goal> Fetch(Goal model);
		BrashActionResult<Goal> Update(Goal model);
		BrashActionResult<Goal> Delete(Goal model);
		BrashQueryResult<Goal> FindWhere(string where);
	}
}