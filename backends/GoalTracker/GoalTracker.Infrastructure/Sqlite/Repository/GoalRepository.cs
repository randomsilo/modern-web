using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using Serilog;
using GoalTracker.Domain.Model;

namespace GoalTracker.Infrastructure.Sqlite.Repository
{
	public class GoalRepository : AskIdRepository<Goal>
	{
		public GoalRepository(IManageDatabase databaseManager, AAskIdRepositorySql repositorySql, ILogger logger) : base(databaseManager, repositorySql, logger)
		{
			
		}
	}
}