
using Brash.Infrastructure;
using Serilog;
using TodoList.Domain.Model;
using TodoList.Domain.Service;

namespace TodoList.Infrastructure.Sqlite.Service
{
	public class TodoNotesService : ITodoNotesService
	{
		protected IAskIdRepository<TodoNotes> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public TodoNotesService(IAskIdRepository<TodoNotes> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<TodoNotes> Create(TodoNotes model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<TodoNotes> Fetch(TodoNotes model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<TodoNotes> Update(TodoNotes model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<TodoNotes> Delete(TodoNotes model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<TodoNotes> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}

		public BrashQueryResult<TodoNotes> FindByParent(int id)
		{
			string where = $"WHERE TodoEntryId = {id}";
			return Repository.FindWhere(where);
		}
	}
}