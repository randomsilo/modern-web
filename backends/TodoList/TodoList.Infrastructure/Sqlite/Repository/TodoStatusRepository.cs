using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using Serilog;
using TodoList.Domain.Model;

namespace TodoList.Infrastructure.Sqlite.Repository
{
	public class TodoStatusRepository : AskIdRepository<TodoStatus>
	{
		public TodoStatusRepository(IManageDatabase databaseManager, AAskIdRepositorySql repositorySql, ILogger logger) : base(databaseManager, repositorySql, logger)
		{
			
		}
	}
}