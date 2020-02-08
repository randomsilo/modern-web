
using System;
using System.Collections.Generic;
using System.Linq;
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
	public class AuthServiceTest
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
		public void CreateUserAccounts()
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

      // - repositories
			var lGAccountRepository = new LGAccountRepository(databaseManager, new LGAccountRepositorySql(), logger);
			var lGPrivilegeRepository = new LGPrivilegeRepository(databaseManager, new LGPrivilegeRepositorySql(), logger);
      var lGFeatureRepository = new LGFeatureRepository(databaseManager, new LGFeatureRepositorySql(), logger);
      var lGAbilityRepository = new LGAbilityRepository(databaseManager, new LGAbilityRepositorySql(), logger);
      var lGRoleRepository = new LGRoleRepository(databaseManager, new LGRoleRepositorySql(), logger);
      var lGTokenRepository = new LGTokenRepository(databaseManager, new LGTokenRepositorySql(), logger);
      Assert.NotNull(lGAccountRepository);
      Assert.NotNull(lGPrivilegeRepository);
      Assert.NotNull(lGFeatureRepository);
      Assert.NotNull(lGAbilityRepository);
      Assert.NotNull(lGRoleRepository);
      Assert.NotNull(lGTokenRepository);

			// - services
			var lGAccountService = new LGAccountService(lGAccountRepository, logger);
      var lGPrivilegeService = new LGPrivilegeService(lGPrivilegeRepository, logger);
      var lGFeatureService = new LGFeatureService(lGFeatureRepository, logger);
      var lGAbilityService = new LGAbilityService(lGAbilityRepository, logger);
      var lGRoleService = new LGRoleService(lGRoleRepository, logger);
      var lGTokenService = new LGTokenService(lGTokenRepository, logger);
			Assert.NotNull(lGAccountService);
      Assert.NotNull(lGPrivilegeService);
			Assert.NotNull(lGFeatureService);
			Assert.NotNull(lGAbilityService);
			Assert.NotNull(lGRoleService);
			Assert.NotNull(lGTokenService);

      // - authService
      var authService = new AuthService( 
        lGAccountService
        , lGTokenService
        , lGPrivilegeService
        , lGFeatureService
        , lGAbilityService
        , lGRoleService
        , logger 
      );

      // - get all features
      var featureModels = lGFeatureService.FindWhere("").Models;

      // - get all abilities
      var abilityModels = lGAbilityService.FindWhere("").Models;

      // - get specific roles
      var roleModels = lGRoleService.FindWhere("").Models;
      var adminRole = roleModels.FirstOrDefault( role => role.ChoiceName == "Administrator");
      var userRole = roleModels.FirstOrDefault( role => role.ChoiceName == "User");
      var auditorRole = roleModels.FirstOrDefault( role => role.ChoiceName == "Auditor");

      // create admin
      LGAccount adminAccount = new LGAccount()
      {
        LastName = "Administator"
        , FirstName = "System"
        , MiddleName = "X"
        , UserName = "System"
        , Email = "x@x.com"
        , Password = "System123!"
        , RoleIdRef = adminRole.LGRoleId
      };
      
      var serviceResult = lGAccountService.Create(adminAccount);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.LGAccountId > 0);
      adminAccount = serviceResult.Model;

      foreach( var feature in featureModels)
      {
        foreach( var ability in abilityModels)
        {
          var priviledge = new LGPrivilege()
          {
            AbilityIdRef = ability.LGAbilityId
            , FeatureIdRef = feature.LGFeatureId
            , LGAccountId = adminAccount.LGAccountId
            , Starts = DateTime.Now
            , Ends = DateTime.Now.AddYears(2)
          };

          var priviledgeCreateResult = lGPrivilegeService.Create(priviledge);
          Assert.True(priviledgeCreateResult.Status == BrashActionStatus.SUCCESS, priviledgeCreateResult.Message);
			    Assert.True(priviledgeCreateResult.Model.LGPrivilegeId > 0);
        }
      }

      // authenticate
      var adminSignIn = new AccountSignin()
      {
        UserName = "System"
        , Password = "System123!"
      };
      var adminAuthResult = authService.Authenticate(adminSignIn);
      Assert.True(adminAuthResult.Status == BrashActionStatus.SUCCESS);
      Assert.NotNull(adminAuthResult.Model);
      Assert.NotNull(adminAuthResult.Model.Account);
      Assert.NotNull(adminAuthResult.Model.Token);
    
      Assert.Equal("Administrator", adminAuthResult.Model.Role);
		}

	}
}