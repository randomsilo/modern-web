
using Brash.Infrastructure;
using Serilog;
using TodoList.Domain.Model;
using TodoList.Domain.Service;

namespace TodoList.Infrastructure.Sqlite.Service
{
	public class TodoEntryService : ITodoEntryService
	{
		protected IAskIdRepository<TodoEntry> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public TodoEntryService(IAskIdRepository<TodoEntry> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<TodoEntry> Create(TodoEntry model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<TodoEntry> Fetch(TodoEntry model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<TodoEntry> Update(TodoEntry model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<TodoEntry> Delete(TodoEntry model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<TodoEntry> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}