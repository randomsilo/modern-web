
using Brash.Infrastructure;
using Serilog;
using GoalTracker.Domain.Model;
using GoalTracker.Domain.Service;

namespace GoalTracker.Infrastructure.Sqlite.Service
{
	public class GoalStateService : IGoalStateService
	{
		protected IAskIdRepository<GoalState> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public GoalStateService(IAskIdRepository<GoalState> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<GoalState> Create(GoalState model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<GoalState> Fetch(GoalState model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<GoalState> Update(GoalState model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<GoalState> Delete(GoalState model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<GoalState> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}