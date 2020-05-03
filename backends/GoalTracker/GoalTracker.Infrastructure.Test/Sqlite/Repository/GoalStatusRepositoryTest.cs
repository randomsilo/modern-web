
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using GoalTracker.Domain.Model;
using GoalTracker.Infrastructure.Sqlite.Repository;
using GoalTracker.Infrastructure.Sqlite.RepositorySql;
using GoalTracker.Infrastructure.Test.Sqlite.Faker;

namespace GoalTracker.Infrastructure.Test.Sqlite.Repository
{
	public class GoalStatusRepositoryTest
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
			var path = "/shop/randomsilo/modern-web/backends/GoalTracker";
			var project = "GoalTracker";
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
			var goalStatusRepository = new GoalStatusRepository(databaseManager, new GoalStatusRepositorySql(), logger);
			Assert.NotNull(goalStatusRepository);

			// faker
			BrashActionResult<GoalStatus> result = null;
			var goalStatusFaker = new GoalStatusFaker(databaseManager, logger);
			Assert.NotNull(goalStatusFaker);

			// create
			var goalStatusCreateModel = goalStatusFaker.GetOne();
			result = goalStatusRepository.Create(goalStatusCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
			Assert.True(result.Model.GoalStatusId > 0);

			// use model with id
			goalStatusCreateModel = result.Model;

			// update
			var goalStatusUpdateModel = goalStatusFaker.GetOne();
			goalStatusUpdateModel.GoalStatusId = goalStatusCreateModel.GoalStatusId;
			result = goalStatusRepository.Update(goalStatusUpdateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// delete
			result = goalStatusRepository.Delete(goalStatusCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// fetch

			// - make fakes
			var fakes = goalStatusFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				result = goalStatusRepository.Create(f);

				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.GoalStatusId >= 0);
				ids.Add(result.Model.GoalStatusId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new GoalStatus()
				{
					GoalStatusId = id
				};

				result = goalStatusRepository.Fetch(model);
				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.GoalStatusId >= 0);
			}
		}

	}
}