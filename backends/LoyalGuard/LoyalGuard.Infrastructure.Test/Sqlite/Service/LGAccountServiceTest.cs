
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
	public class LGAccountServiceTest
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
			var lGAccountRepository = new LGAccountRepository(databaseManager, new LGAccountRepositorySql(), logger);
			Assert.NotNull(lGAccountRepository);

			// - service
			var lGAccountService = new LGAccountService(lGAccountRepository, logger);
			Assert.NotNull(lGAccountService);

			// faker
			BrashActionResult<LGAccount> serviceResult = null;
			var lGAccountFaker = new LGAccountFaker(databaseManager, logger);
			Assert.NotNull(lGAccountFaker);

			// create
			var lGAccountCreateModel = lGAccountFaker.GetOne();
			serviceResult = lGAccountService.Create(lGAccountCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.LGAccountId > 0);

			// use model with id
			lGAccountCreateModel = serviceResult.Model;

			// update
			var lGAccountUpdateModel = lGAccountFaker.GetOne();
			lGAccountUpdateModel.LGAccountId = lGAccountCreateModel.LGAccountId;
			serviceResult = lGAccountService.Update(lGAccountUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = lGAccountService.Delete(lGAccountCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = lGAccountFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = lGAccountService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGAccountId >= 0);
				ids.Add(serviceResult.Model.LGAccountId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new LGAccount()
				{
					LGAccountId = id
				};

				serviceResult = lGAccountService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.LGAccountId >= 0);
			}
		}

	}
}