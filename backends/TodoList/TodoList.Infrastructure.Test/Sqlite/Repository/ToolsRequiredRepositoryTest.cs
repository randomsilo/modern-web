
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
	public class ToolsRequiredRepositoryTest
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
			var toolsRequiredRepository = new ToolsRequiredRepository(databaseManager, new ToolsRequiredRepositorySql(), logger);
			Assert.NotNull(toolsRequiredRepository);

			// faker
			BrashActionResult<ToolsRequired> result = null;
			var toolsRequiredFaker = new ToolsRequiredFaker(databaseManager, logger);
			Assert.NotNull(toolsRequiredFaker);

			// create
			var toolsRequiredCreateModel = toolsRequiredFaker.GetOne();
			result = toolsRequiredRepository.Create(toolsRequiredCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
			Assert.True(result.Model.ToolsRequiredId > 0);

			// use model with id
			toolsRequiredCreateModel = result.Model;

			// update
			var toolsRequiredUpdateModel = toolsRequiredFaker.GetOne();
			toolsRequiredUpdateModel.ToolsRequiredId = toolsRequiredCreateModel.ToolsRequiredId;
			result = toolsRequiredRepository.Update(toolsRequiredUpdateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// delete
			result = toolsRequiredRepository.Delete(toolsRequiredCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// fetch

			// - make fakes
			var fakes = toolsRequiredFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				result = toolsRequiredRepository.Create(f);

				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.ToolsRequiredId >= 0);
				ids.Add(result.Model.ToolsRequiredId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new ToolsRequired()
				{
					ToolsRequiredId = id
				};

				result = toolsRequiredRepository.Fetch(model);
				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.ToolsRequiredId >= 0);
			}
		}

	}
}