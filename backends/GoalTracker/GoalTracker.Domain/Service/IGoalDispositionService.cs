
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using GoalTracker.Domain.Model;

namespace GoalTracker.Domain.Service
{
	public interface IGoalDispositionService 
	{
		BrashActionResult<GoalDisposition> Create(GoalDisposition model);
		BrashActionResult<GoalDisposition> Fetch(GoalDisposition model);
		BrashActionResult<GoalDisposition> Update(GoalDisposition model);
		BrashActionResult<GoalDisposition> Delete(GoalDisposition model);
		BrashQueryResult<GoalDisposition> FindWhere(string where);
	}
}