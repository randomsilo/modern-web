
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using GoalTracker.Domain.Model;

namespace GoalTracker.Domain.Service
{
	public interface IGoalStatusService 
	{
		BrashActionResult<GoalStatus> Create(GoalStatus model);
		BrashActionResult<GoalStatus> Fetch(GoalStatus model);
		BrashActionResult<GoalStatus> Update(GoalStatus model);
		BrashActionResult<GoalStatus> Delete(GoalStatus model);
		BrashQueryResult<GoalStatus> FindWhere(string where);
	}
}