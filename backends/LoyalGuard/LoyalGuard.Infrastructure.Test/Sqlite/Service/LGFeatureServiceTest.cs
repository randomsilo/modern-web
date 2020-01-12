
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using LoyalGuard.Domain.Model;
using LoyalGuard.Infrastructure.Sqlite.Repository;
using LoyalGuard.Infrastructure.Sqlite.RepositorySql;
using LoyalGuard.Infrastructure.Sqlite.Service;
using LoyalGuard.Infrastructure.Test.Sqlite.Faker;

namespace LoyalGuard.Infrastructure.Test.Sqlite.Service
{
	public class LGFeatureServiceTest
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
			var path = "/shop/randomsilo/modern-web/backends/LoyalGuard";
			var project = "LoyalGuard";
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
			var lGFeatureRepository = new LGFeatureRepository(databaseManager, new LGFeatureRepositorySql(), logger);
			Assert.NotNull(lGFeatureRepository);

			// - service
			var lGFeatureService = new LGFeatureService(lGFeatureRepository, logger);
			Assert.NotNull(lGFeatureService);

			// faker
			BrashActionResult<LGFeature> serviceResult = null;
			var lGFeatureFaker = new LGFeatureFaker(databaseManager, logger);
			Assert.NotNull(lGFeatureFaker);

			// create
			var lGFeatureCreateModel = lGFeatureFaker.GetOne();
			serviceResult = lGFeatureService.Create(lGFeatureCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.LGFeatureId > 0);

			// use model with id
			lGFeatureCreateModel = serviceResult.Model;

			// update
			var lGFeatureUpdateModel = lGFeatureFaker.GetOne();
			lGFeatureUpdateModel.LGFeatureId = lGFeatureCreateModel.LGFeatureId;
			serviceResult = lGFeatureService.Update(lGFeatureUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = lGFeatureService.Delete(lGFeatureCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = lGFeatureFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = lGFeatureService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGFeatureId >= 0);
				ids.Add(serviceResult.Model.LGFeatureId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new LGFeature()
				{
					LGFeatureId = id
				};

				serviceResult = lGFeatureService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGFeatureId >= 0);
			}
		}

	}
}