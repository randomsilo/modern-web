
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using LoyalGuard.Domain.Model;
using LoyalGuard.Infrastructure.Sqlite.Repository;
using LoyalGuard.Infrastructure.Sqlite.RepositorySql;
using LoyalGuard.Infrastructure.Test.Sqlite.Faker;

namespace LoyalGuard.Infrastructure.Test.Sqlite.Repository
{
	public class LGPrivilegeRepositoryTest
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
			var lGPrivilegeRepository = new LGPrivilegeRepository(databaseManager, new LGPrivilegeRepositorySql(), logger);
			Assert.NotNull(lGPrivilegeRepository);

			// faker
			BrashActionResult<LGPrivilege> result = null;
			var lGPrivilegeFaker = new LGPrivilegeFaker(databaseManager, logger);
			Assert.NotNull(lGPrivilegeFaker);

			// create
			var lGPrivilegeCreateModel = lGPrivilegeFaker.GetOne();
			result = lGPrivilegeRepository.Create(lGPrivilegeCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
			Assert.True(result.Model.LGPrivilegeId > 0);

			// use model with id
			lGPrivilegeCreateModel = result.Model;

			// update
			var lGPrivilegeUpdateModel = lGPrivilegeFaker.GetOne();
			lGPrivilegeUpdateModel.LGPrivilegeId = lGPrivilegeCreateModel.LGPrivilegeId;
			result = lGPrivilegeRepository.Update(lGPrivilegeUpdateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// delete
			result = lGPrivilegeRepository.Delete(lGPrivilegeCreateModel);
			Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);

			// fetch

			// - make fakes
			var fakes = lGPrivilegeFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				result = lGPrivilegeRepository.Create(f);

				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.LGPrivilegeId >= 0);
				ids.Add(result.Model.LGPrivilegeId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new LGPrivilege()
				{
					LGPrivilegeId = id
				};

				result = lGPrivilegeRepository.Fetch(model);
				Assert.True(result.Status == BrashActionStatus.SUCCESS, result.Message);
				Assert.True(result.Model.LGPrivilegeId >= 0);
			}
		}

	}
}