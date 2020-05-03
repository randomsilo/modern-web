using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using Serilog;
using GoalTracker.Domain.Model;

namespace GoalTracker.Infrastructure.Sqlite.Repository
{
	public class GoalDispositionRepository : AskIdRepository<GoalDisposition>
	{
		public GoalDispositionRepository(IManageDatabase databaseManager, AAskIdRepositorySql repositorySql, ILogger logger) : base(databaseManager, repositorySql, logger)
		{
			
		}
	}
}