
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
	public class TodoStatusServiceTest
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
			var todoStatusRepository = new TodoStatusRepository(databaseManager, new TodoStatusRepositorySql(), logger);
			Assert.NotNull(todoStatusRepository);

			// - service
			var todoStatusService = new TodoStatusService(todoStatusRepository, logger);
			Assert.NotNull(todoStatusService);

			// faker
			BrashActionResult<TodoStatus> serviceResult = null;
			var todoStatusFaker = new TodoStatusFaker(databaseManager, logger);
			Assert.NotNull(todoStatusFaker);

			// create
			var todoStatusCreateModel = todoStatusFaker.GetOne();
			serviceResult = todoStatusService.Create(todoStatusCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.TodoStatusId > 0);

			// use model with id
			todoStatusCreateModel = serviceResult.Model;

			// update
			var todoStatusUpdateModel = todoStatusFaker.GetOne();
			todoStatusUpdateModel.TodoStatusId = todoStatusCreateModel.TodoStatusId;
			serviceResult = todoStatusService.Update(todoStatusUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = todoStatusService.Delete(todoStatusCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = todoStatusFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = todoStatusService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoStatusId >= 0);
				ids.Add(serviceResult.Model.TodoStatusId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new TodoStatus()
				{
					TodoStatusId = id
				};

				serviceResult = todoStatusService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoStatusId >= 0);
			}
		}

	}
}