using Autofac;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using TodoList.Infrastructure.Sqlite.Repository;
using TodoList.Infrastructure.Sqlite.RepositorySql;
using TodoList.Infrastructure.Sqlite.Service;

namespace TodoList.Api
{
    public class BrashConfigure
    {
        public static void LoadContainer(ContainerBuilder containerBuilder)
        {
            var path = System.IO.Directory.GetCurrentDirectory();
			var name = "TodoList";
			
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
			
			// Container Registar: TodoStatus
			containerBuilder.Register<TodoStatusRepositorySql>((c) => { return new TodoStatusRepositorySql(); });
			containerBuilder.Register<TodoStatusRepository>((c) => { return new TodoStatusRepository( c.Resolve<IManageDatabase>(), c.Resolve<TodoStatusRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<TodoStatusService>((c) => { return new TodoStatusService( c.Resolve<TodoStatusRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: TodoEntry
			containerBuilder.Register<TodoEntryRepositorySql>((c) => { return new TodoEntryRepositorySql(); });
			containerBuilder.Register<TodoEntryRepository>((c) => { return new TodoEntryRepository( c.Resolve<IManageDatabase>(), c.Resolve<TodoEntryRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<TodoEntryService>((c) => { return new TodoEntryService( c.Resolve<TodoEntryRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: ToolsRequired
			containerBuilder.Register<ToolsRequiredRepositorySql>((c) => { return new ToolsRequiredRepositorySql(); });
			containerBuilder.Register<ToolsRequiredRepository>((c) => { return new ToolsRequiredRepository( c.Resolve<IManageDatabase>(), c.Resolve<ToolsRequiredRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<ToolsRequiredService>((c) => { return new ToolsRequiredService( c.Resolve<ToolsRequiredRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// Container Registar: TodoNotes
			containerBuilder.Register<TodoNotesRepositorySql>((c) => { return new TodoNotesRepositorySql(); });
			containerBuilder.Register<TodoNotesRepository>((c) => { return new TodoNotesRepository( c.Resolve<IManageDatabase>(), c.Resolve<TodoNotesRepositorySql>(), c.Resolve<Serilog.ILogger>()); });
			containerBuilder.Register<TodoNotesService>((c) => { return new TodoNotesService( c.Resolve<TodoNotesRepository>(), c.Resolve<Serilog.ILogger>()); });
			
			// BasicAuth
			containerBuilder.Register<IBrashApiAuthService>((c) => {
				return new BrashApiAuthService().AddAuthAccount(
					new BrashApiAuthModel(){
						ApiAuthId = 1
						, ApiAuthName = "API_TODOLIST"
						, ApiAuthPass = "API_THINGS_TO_DO"
					});
				});
		}
	}
}