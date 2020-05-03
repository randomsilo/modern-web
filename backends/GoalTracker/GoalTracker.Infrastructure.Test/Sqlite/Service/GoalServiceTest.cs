
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using GoalTracker.Domain.Model;
using GoalTracker.Infrastructure.Sqlite.Repository;
using GoalTracker.Infrastructure.Sqlite.RepositorySql;
using GoalTracker.Infrastructure.Sqlite.Service;
using GoalTracker.Infrastructure.Test.Sqlite.Faker;

namespace GoalTracker.Infrastructure.Test.Sqlite.Service
{
	public class GoalServiceTest
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
			var goalRepository = new GoalRepository(databaseManager, new GoalRepositorySql(), logger);
			Assert.NotNull(goalRepository);

			// - service
			var goalService = new GoalService(goalRepository, logger);
			Assert.NotNull(goalService);

			// faker
			BrashActionResult<Goal> serviceResult = null;
			var goalFaker = new GoalFaker(databaseManager, logger);
			Assert.NotNull(goalFaker);

			// create
			var goalCreateModel = goalFaker.GetOne();
			serviceResult = goalService.Create(goalCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.GoalId > 0);

			// use model with id
			goalCreateModel = serviceResult.Model;

			// update
			var goalUpdateModel = goalFaker.GetOne();
			goalUpdateModel.GoalId = goalCreateModel.GoalId;
			serviceResult = goalService.Update(goalUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = goalService.Delete(goalCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = goalFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = goalService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.GoalId >= 0);
				ids.Add(serviceResult.Model.GoalId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new Goal()
				{
					GoalId = id
				};

				serviceResult = goalService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.GoalId >= 0);
			}
		}

	}
}