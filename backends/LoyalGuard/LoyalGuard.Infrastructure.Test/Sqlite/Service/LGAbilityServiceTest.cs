
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
	public class LGAbilityServiceTest
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
			var lGAbilityRepository = new LGAbilityRepository(databaseManager, new LGAbilityRepositorySql(), logger);
			Assert.NotNull(lGAbilityRepository);

			// - service
			var lGAbilityService = new LGAbilityService(lGAbilityRepository, logger);
			Assert.NotNull(lGAbilityService);

			// faker
			BrashActionResult<LGAbility> serviceResult = null;
			var lGAbilityFaker = new LGAbilityFaker(databaseManager, logger);
			Assert.NotNull(lGAbilityFaker);

			// create
			var lGAbilityCreateModel = lGAbilityFaker.GetOne();
			serviceResult = lGAbilityService.Create(lGAbilityCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.LGAbilityId > 0);

			// use model with id
			lGAbilityCreateModel = serviceResult.Model;

			// update
			var lGAbilityUpdateModel = lGAbilityFaker.GetOne();
			lGAbilityUpdateModel.LGAbilityId = lGAbilityCreateModel.LGAbilityId;
			serviceResult = lGAbilityService.Update(lGAbilityUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = lGAbilityService.Delete(lGAbilityCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = lGAbilityFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = lGAbilityService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGAbilityId >= 0);
				ids.Add(serviceResult.Model.LGAbilityId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new LGAbility()
				{
					LGAbilityId = id
				};

				serviceResult = lGAbilityService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGAbilityId >= 0);
			}
		}

	}
}