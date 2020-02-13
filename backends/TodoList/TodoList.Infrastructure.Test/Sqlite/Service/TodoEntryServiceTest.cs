
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using TodoList.Domain.Model;
using TodoList.Infrastructure.Sqlite.Repository;
using TodoList.Infrastructure.Sqlite.RepositorySql;
using TodoList.Infrastructure.Sqlite.Service;
using TodoList.Infrastructure.Test.Sqlite.Faker;

namespace TodoList.Infrastructure.Test.Sqlite.Service
{
	public class TodoEntryServiceTest
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
			ILogger logger = GetLogger($"{outputPath}/{MethodBase.GetCurrentMethod()}.log");
			
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

			// - service
			var todoEntryService = new TodoEntryService(todoEntryRepository, logger);
			Assert.NotNull(todoEntryService);

			// faker
			BrashActionResult<TodoEntry> serviceResult = null;
			var todoEntryFaker = new TodoEntryFaker(databaseManager, logger);
			Assert.NotNull(todoEntryFaker);

			// create
			var todoEntryCreateModel = todoEntryFaker.GetOne();
			serviceResult = todoEntryService.Create(todoEntryCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.TodoEntryId > 0);

			// use model with id
			todoEntryCreateModel = serviceResult.Model;

			// update
			var todoEntryUpdateModel = todoEntryFaker.GetOne();
			todoEntryUpdateModel.TodoEntryId = todoEntryCreateModel.TodoEntryId;
			serviceResult = todoEntryService.Update(todoEntryUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = todoEntryService.Delete(todoEntryCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = todoEntryFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = todoEntryService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoEntryId >= 0);
				ids.Add(serviceResult.Model.TodoEntryId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new TodoEntry()
				{
					TodoEntryId = id
				};

				serviceResult = todoEntryService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoEntryId >= 0);
			}
		}

	}
}