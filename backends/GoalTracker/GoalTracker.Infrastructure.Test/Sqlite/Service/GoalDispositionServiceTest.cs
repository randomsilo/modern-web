
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
	public class GoalDispositionServiceTest
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
			var goalDispositionRepository = new GoalDispositionRepository(databaseManager, new GoalDispositionRepositorySql(), logger);
			Assert.NotNull(goalDispositionRepository);

			// - service
			var goalDispositionService = new GoalDispositionService(goalDispositionRepository, logger);
			Assert.NotNull(goalDispositionService);

			// faker
			BrashActionResult<GoalDisposition> serviceResult = null;
			var goalDispositionFaker = new GoalDispositionFaker(databaseManager, logger);
			Assert.NotNull(goalDispositionFaker);

			// create
			var goalDispositionCreateModel = goalDispositionFaker.GetOne();
			serviceResult = goalDispositionService.Create(goalDispositionCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.GoalDispositionId > 0);

			// use model with id
			goalDispositionCreateModel = serviceResult.Model;

			// update
			var goalDispositionUpdateModel = goalDispositionFaker.GetOne();
			goalDispositionUpdateModel.GoalDispositionId = goalDispositionCreateModel.GoalDispositionId;
			serviceResult = goalDispositionService.Update(goalDispositionUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = goalDispositionService.Delete(goalDispositionCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = goalDispositionFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = goalDispositionService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.GoalDispositionId >= 0);
				ids.Add(serviceResult.Model.GoalDispositionId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new GoalDisposition()
				{
					GoalDispositionId = id
				};

				serviceResult = goalDispositionService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.GoalDispositionId >= 0);
			}
		}

	}
}