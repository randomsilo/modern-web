
using Brash.Infrastructure;
using Serilog;
using GoalTracker.Domain.Model;
using GoalTracker.Domain.Service;

namespace GoalTracker.Infrastructure.Sqlite.Service
{
	public class GoalStatusService : IGoalStatusService
	{
		protected IAskIdRepository<GoalStatus> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public GoalStatusService(IAskIdRepository<GoalStatus> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<GoalStatus> Create(GoalStatus model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<GoalStatus> Fetch(GoalStatus model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<GoalStatus> Update(GoalStatus model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<GoalStatus> Delete(GoalStatus model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<GoalStatus> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}