
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
	public class ToolsRequiredServiceTest
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
			var toolsRequiredRepository = new ToolsRequiredRepository(databaseManager, new ToolsRequiredRepositorySql(), logger);
			Assert.NotNull(toolsRequiredRepository);

			// - service
			var toolsRequiredService = new ToolsRequiredService(toolsRequiredRepository, logger);
			Assert.NotNull(toolsRequiredService);

			// faker
			BrashActionResult<ToolsRequired> serviceResult = null;
			var toolsRequiredFaker = new ToolsRequiredFaker(databaseManager, logger);
			Assert.NotNull(toolsRequiredFaker);

			// create
			var toolsRequiredCreateModel = toolsRequiredFaker.GetOne();
			serviceResult = toolsRequiredService.Create(toolsRequiredCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.ToolsRequiredId > 0);

			// use model with id
			toolsRequiredCreateModel = serviceResult.Model;

			// update
			var toolsRequiredUpdateModel = toolsRequiredFaker.GetOne();
			toolsRequiredUpdateModel.ToolsRequiredId = toolsRequiredCreateModel.ToolsRequiredId;
			serviceResult = toolsRequiredService.Update(toolsRequiredUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = toolsRequiredService.Delete(toolsRequiredCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = toolsRequiredFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = toolsRequiredService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.ToolsRequiredId >= 0);
				ids.Add(serviceResult.Model.ToolsRequiredId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new ToolsRequired()
				{
					ToolsRequiredId = id
				};

				serviceResult = toolsRequiredService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.ToolsRequiredId >= 0);
			}
		}

	}
}