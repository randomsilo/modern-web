
using Brash.Infrastructure;
using Serilog;
using TodoList.Domain.Model;
using TodoList.Domain.Service;

namespace TodoList.Infrastructure.Sqlite.Service
{
	public class ToolsRequiredService : IToolsRequiredService
	{
		protected IAskIdRepository<ToolsRequired> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public ToolsRequiredService(IAskIdRepository<ToolsRequired> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<ToolsRequired> Create(ToolsRequired model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<ToolsRequired> Fetch(ToolsRequired model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<ToolsRequired> Update(ToolsRequired model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<ToolsRequired> Delete(ToolsRequired model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<ToolsRequired> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}

		public BrashQueryResult<ToolsRequired> FindByParent(int id)
		{
			string where = $"WHERE TodoEntryId = {id}";
			return Repository.FindWhere(where);
		}
	}
}