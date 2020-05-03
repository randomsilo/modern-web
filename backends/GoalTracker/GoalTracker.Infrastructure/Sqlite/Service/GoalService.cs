
using Brash.Infrastructure;
using Serilog;
using GoalTracker.Domain.Model;
using GoalTracker.Domain.Service;

namespace GoalTracker.Infrastructure.Sqlite.Service
{
	public class GoalService : IGoalService
	{
		protected IAskIdRepository<Goal> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public GoalService(IAskIdRepository<Goal> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<Goal> Create(Goal model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<Goal> Fetch(Goal model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<Goal> Update(Goal model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<Goal> Delete(Goal model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<Goal> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}