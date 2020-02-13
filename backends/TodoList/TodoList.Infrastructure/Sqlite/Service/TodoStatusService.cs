
using Brash.Infrastructure;
using Serilog;
using TodoList.Domain.Model;
using TodoList.Domain.Service;

namespace TodoList.Infrastructure.Sqlite.Service
{
	public class TodoStatusService : ITodoStatusService
	{
		protected IAskIdRepository<TodoStatus> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public TodoStatusService(IAskIdRepository<TodoStatus> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<TodoStatus> Create(TodoStatus model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<TodoStatus> Fetch(TodoStatus model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<TodoStatus> Update(TodoStatus model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<TodoStatus> Delete(TodoStatus model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<TodoStatus> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}