using Autofac;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using LoyalGuard.Infrastructure.Sqlite.Repository;
using LoyalGuard.Infrastructure.Sqlite.RepositorySql;
using LoyalGuard.Infrastructure.Sqlite.Service;

namespace LoyalGuard.Api
{
    public class BrashConfigure
    {
        public static void LoadContainer(ContainerBuilder containerBuilder)
        {
            var path = System.IO.Directory.GetCurrentDirectory();
			var name = "LoyalGuard";
			
			// Logger
			var logfilename = $"{path}/{name}.log";
            var logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.File(logfilename, rollingInterval: RollingInterval.Day)
				.CreateLogger();
			
			// Database Configuration
			var databaseFile = $"../sql/sqlite/{name}.webapi.sqlite";
			bool databaseExists = System.IO.File.Exists(databaseFile);
			var databaseContext = new DatabaseContext(
			        $"Data Source={databaseFile}" 
			        , $"{name}Db"
			        , $"{name}Schema"
			        , $"../sql/sqlite/ALL.sql");

            // Database Manager
            var databaseManager = new DatabaseManager(databaseContext);
            if (!databaseExists)
            {
                databaseManager.CreateDatabase();
            }
			
            // Container Registar: Database 
            containerBuilder.RegisterInstance(logger).As<Serilog.ILogger>();
            containerBuilder.RegisterInstance(databaseContext).As<IDatabaseContext>();
            containerBuilder.RegisterInstance(databaseManager).As<IManageDatabase>();
			
			// Container Registar: LGRole
			containerBuilder.Register<LGRoleRepositorySql>((c) => { return new LGRoleRepositorySql(); });
			containerBuilder.Register<LGRoleRepository>((c) => { return new LGRoleRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGRoleRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGRoleService>((c) => { return new LGRoleService( c.Resolve<LGRoleRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: LGFeature
			containerBuilder.Register<LGFeatureRepositorySql>((c) => { return new LGFeatureRepositorySql(); });
			containerBuilder.Register<LGFeatureRepository>((c) => { return new LGFeatureRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGFeatureRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGFeatureService>((c) => { return new LGFeatureService( c.Resolve<LGFeatureRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: LGAbility
			containerBuilder.Register<LGAbilityRepositorySql>((c) => { return new LGAbilityRepositorySql(); });
			containerBuilder.Register<LGAbilityRepository>((c) => { return new LGAbilityRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGAbilityRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGAbilityService>((c) => { return new LGAbilityService( c.Resolve<LGAbilityRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: LGAccount
			containerBuilder.Register<LGAccountRepositorySql>((c) => { return new LGAccountRepositorySql(); });
			containerBuilder.Register<LGAccountRepository>((c) => { return new LGAccountRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGAccountRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGAccountService>((c) => { return new LGAccountService( c.Resolve<LGAccountRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: LGPrivilege
			containerBuilder.Register<LGPrivilegeRepositorySql>((c) => { return new LGPrivilegeRepositorySql(); });
			containerBuilder.Register<LGPrivilegeRepository>((c) => { return new LGPrivilegeRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGPrivilegeRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGPrivilegeService>((c) => { return new LGPrivilegeService( c.Resolve<LGPrivilegeRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: LGToken
			containerBuilder.Register<LGTokenRepositorySql>((c) => { return new LGTokenRepositorySql(); });
			containerBuilder.Register<LGTokenRepository>((c) => { return new LGTokenRepository( c.Resolve<IManageDatabase>(), c.Resolve<LGTokenRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<LGTokenService>((c) => { return new LGTokenService( c.Resolve<LGTokenRepository>(), c.Resolve<Serilog.ILogger>()); });

      // AuthService
      containerBuilder.Register<AuthService>((c) => { return new AuthService( 
        c.Resolve<LGAccountService>()
        , c.Resolve<LGTokenService>()
        , c.Resolve<LGPrivilegeService>()
        , c.Resolve<LGFeatureService>()
        , c.Resolve<LGAbilityService>()
        , c.Resolve<LGRoleService>()
        , c.Resolve<Serilog.ILogger>()); 
      });
			
			// BasicAuth
			containerBuilder.Register<IBrashApiAuthService>((c) => {
				return new BrashApiAuthService().AddAuthAccount(
					new BrashApiAuthModel(){
						ApiAuthId = 1
						, ApiAuthName = "API_LOYALGUARD"
						, ApiAuthPass = "API_TWO_IF_BY_SEA"
					});
				});
		}
	}
}