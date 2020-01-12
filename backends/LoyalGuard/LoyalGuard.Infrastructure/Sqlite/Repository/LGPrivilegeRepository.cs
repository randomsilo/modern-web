using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using Serilog;
using LoyalGuard.Domain.Model;

namespace LoyalGuard.Infrastructure.Sqlite.Repository
{
	public class LGPrivilegeRepository : AskIdRepository<LGPrivilege>
	{
		public LGPrivilegeRepository(IManageDatabase databaseManager, AAskIdRepositorySql repositorySql, ILogger logger) : base(databaseManager, repositorySql, logger)
		{
			
		}
	}
}