
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using GoalTracker.Domain.Model;

namespace GoalTracker.Domain.Service
{
	public interface IGoalStateService 
	{
		BrashActionResult<GoalState> Create(GoalState model);
		BrashActionResult<GoalState> Fetch(GoalState model);
		BrashActionResult<GoalState> Update(GoalState model);
		BrashActionResult<GoalState> Delete(GoalState model);
		BrashQueryResult<GoalState> FindWhere(string where);
	}
}