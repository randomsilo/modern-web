
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
	public class LGTokenServiceTest
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
			var lGTokenRepository = new LGTokenRepository(databaseManager, new LGTokenRepositorySql(), logger);
			Assert.NotNull(lGTokenRepository);

			// - service
			var lGTokenService = new LGTokenService(lGTokenRepository, logger);
			Assert.NotNull(lGTokenService);

			// faker
			BrashActionResult<LGToken> serviceResult = null;
			var lGTokenFaker = new LGTokenFaker(databaseManager, logger);
			Assert.NotNull(lGTokenFaker);

			// create
			var lGTokenCreateModel = lGTokenFaker.GetOne();
			serviceResult = lGTokenService.Create(lGTokenCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.LGTokenId > 0);

			// use model with id
			lGTokenCreateModel = serviceResult.Model;

			// update
			var lGTokenUpdateModel = lGTokenFaker.GetOne();
			lGTokenUpdateModel.LGTokenId = lGTokenCreateModel.LGTokenId;
			serviceResult = lGTokenService.Update(lGTokenUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = lGTokenService.Delete(lGTokenCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = lGTokenFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = lGTokenService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGTokenId >= 0);
				ids.Add(serviceResult.Model.LGTokenId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new LGToken()
				{
					LGTokenId = id
				};

				serviceResult = lGTokenService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGTokenId >= 0);
			}
		}

	}
}