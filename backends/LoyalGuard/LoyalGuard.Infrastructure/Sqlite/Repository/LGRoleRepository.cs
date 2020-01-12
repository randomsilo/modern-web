using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using Serilog;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Infrastructure.Sqlite.Repository
{
	public class LGRoleRepository : AskIdRepository<LGRole>
	{
		public LGRoleRepository(IManageDatabase databaseManager, AAskIdRepositorySql repositorySql, ILogger logger) : base(databaseManager, repositorySql, logger)
		{
			
		}
	}
}