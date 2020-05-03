
using Brash.Infrastructure;
using Serilog;
using GoalTracker.Domain.Model;
using GoalTracker.Domain.Service;

namespace GoalTracker.Infrastructure.Sqlite.Service
{
	public class GoalDispositionService : IGoalDispositionService
	{
		protected IAskIdRepository<GoalDisposition> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public GoalDispositionService(IAskIdRepository<GoalDisposition> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<GoalDisposition> Create(GoalDisposition model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<GoalDisposition> Fetch(GoalDisposition model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<GoalDisposition> Update(GoalDisposition model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<GoalDisposition> Delete(GoalDisposition model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<GoalDisposition> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}