using Autofac;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using GoalTracker.Infrastructure.Sqlite.Repository;
using GoalTracker.Infrastructure.Sqlite.RepositorySql;
using GoalTracker.Infrastructure.Sqlite.Service;

namespace GoalTracker.Api
{
    public class BrashConfigure
    {
        public static void LoadContainer(ContainerBuilder containerBuilder)
        {
            var path = System.IO.Directory.GetCurrentDirectory();
			var name = "GoalTracker";
			
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
			
			// Container Registar: GoalDisposition
			containerBuilder.Register<GoalDispositionRepositorySql>((c) => { return new GoalDispositionRepositorySql(); });
			containerBuilder.Register<GoalDispositionRepository>((c) => { return new GoalDispositionRepository( c.Resolve<IManageDatabase>(), c.Resolve<GoalDispositionRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<GoalDispositionService>((c) => { return new GoalDispositionService( c.Resolve<GoalDispositionRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: GoalState
			containerBuilder.Register<GoalStateRepositorySql>((c) => { return new GoalStateRepositorySql(); });
			containerBuilder.Register<GoalStateRepository>((c) => { return new GoalStateRepository( c.Resolve<IManageDatabase>(), c.Resolve<GoalStateRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<GoalStateService>((c) => { return new GoalStateService( c.Resolve<GoalStateRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: GoalStatus
			containerBuilder.Register<GoalStatusRepositorySql>((c) => { return new GoalStatusRepositorySql(); });
			containerBuilder.Register<GoalStatusRepository>((c) => { return new GoalStatusRepository( c.Resolve<IManageDatabase>(), c.Resolve<GoalStatusRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<GoalStatusService>((c) => { return new GoalStatusService( c.Resolve<GoalStatusRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: Goal
			containerBuilder.Register<GoalRepositorySql>((c) => { return new GoalRepositorySql(); });
			containerBuilder.Register<GoalRepository>((c) => { return new GoalRepository( c.Resolve<IManageDatabase>(), c.Resolve<GoalRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<GoalService>((c) => { return new GoalService( c.Resolve<GoalRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// BasicAuth
			containerBuilder.Register<IBrashApiAuthService>((c) => {
				return new BrashApiAuthService().AddAuthAccount(
					new BrashApiAuthModel(){
						ApiAuthId = 1
						, ApiAuthName = "API_GOALTRACKER"
						, ApiAuthPass = "API_GOALTRACKER"
					});
				});
		}
	}
}