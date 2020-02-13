
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using TodoList.Domain.Model;
using TodoList.Infrastructure.Sqlite.Repository;
using TodoList.Infrastructure.Sqlite.RepositorySql;
using TodoList.Infrastructure.Test.Sqlite.Faker;

namespace TodoList.Infrastructure.Test.Sqlite.Repository
{
	public class TodoEntryRepositoryTest
	{
		public string GetDatabase(string path, MethodBase methodBase)
		{
			string dbName = $"{methodBase.ReflectedType.Name}_{methodBase.Name}";
			string databaseFile = $"{path}/{dbName}.sqlite";
			System.IO.File.Delete(databaseFile);

			return databaseFile;
		}

		public static ILogger GetLogger(string filename)
		{
			return new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.File($"{filename}", rollingInterval: RollingInterval.Day)
				.CreateLogger();
		}

		[Fact]
		public void CreateUpdateDeleteFetch()
		{
			// file system
			var path = "/shop/randomsilo/modern-web/backends/TodoList";
			var project = "TodoList";
			var outputPath = $"{path}/{project}.Infrastructure.Test/TestOutput/";
			var databaseFile = GetDatabase(outputPath, MethodBase.GetCurrentMethod());
			
			// logger
			ILogger logger = GetLogger($"{outputPath}/{MethodBase.GetCurrentMethod().ReflectedType.Name}_{MethodBase.GetCurrentMethod().Name}.log");
			
			// database setup

			// - context
			IDatabaseContext databaseContext = new DatabaseContext(
				$"Data Source={databaseFile}" 
				, "TestDb"
				, "TestSchema"
				, $"{path}/sql/sqlite/ALL.sql"
			);
			Assert.NotNull(databaseContext);

			// - manager
			IManageDatabase databaseManager = new DatabaseManager(databaseContext);
			Assert.NotNull(databaseManager);

			// - create tables
			databaseManager.CreateDatabase();

			// - repository
			var todoEntryRepository = new TodoEntryRepository(databaseManager, new TodoEntryRepositorySql(), logger);
			Assert.NotNull(todoEntryRepository);

			// faker
			BrashActionResult<TodoEntry> result = null;
			var todoEntryFaker = new TodoEntryFaker(databaseManager, logger);
			Assert.NotNull(todoEntryFaker);

			// create
			var todoEntryCreateModel = todoEntryFaker.GetOne();
			result = todoEntryRepository.Create(todoEntryCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
			Assert.True(result.Model.TodoEntryId > 0);

			// use model with id
			todoEntryCreateModel = result.Model;

			// update
			var todoEntryUpdateModel = todoEntryFaker.GetOne();
			todoEntryUpdateModel.TodoEntryId = todoEntryCreateModel.TodoEntryId;
			result = todoEntryRepository.Update(todoEntryUpdateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// delete
			result = todoEntryRepository.Delete(todoEntryCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// fetch

			// - make fakes
			var fakes = todoEntryFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				result = todoEntryRepository.Create(f);

				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.TodoEntryId >= 0);
				ids.Add(result.Model.TodoEntryId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new TodoEntry()
				{
					TodoEntryId = id
				};

				result = todoEntryRepository.Fetch(model);
				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.TodoEntryId >= 0);
			}
		}

	}
}