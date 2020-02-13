
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Serilog;
using Brash.Infrastructure;
using Brash.Infrastructure.Sqlite;
using TodoList.Domain.Model;
using TodoList.Infrastructure.Sqlite.Repository;
using TodoList.Infrastructure.Sqlite.RepositorySql;
using TodoList.Infrastructure.Sqlite.Service;
using TodoList.Infrastructure.Test.Sqlite.Faker;

namespace TodoList.Infrastructure.Test.Sqlite.Service
{
	public class TodoNotesServiceTest
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
			var path = "/shop/randomsilo/modern-web/backends/TodoList";
			var project = "TodoList";
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
			var todoNotesRepository = new TodoNotesRepository(databaseManager, new TodoNotesRepositorySql(), logger);
			Assert.NotNull(todoNotesRepository);

			// - service
			var todoNotesService = new TodoNotesService(todoNotesRepository, logger);
			Assert.NotNull(todoNotesService);

			// faker
			BrashActionResult<TodoNotes> serviceResult = null;
			var todoNotesFaker = new TodoNotesFaker(databaseManager, logger);
			Assert.NotNull(todoNotesFaker);

			// create
			var todoNotesCreateModel = todoNotesFaker.GetOne();
			serviceResult = todoNotesService.Create(todoNotesCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
			Assert.True(serviceResult.Model.TodoNotesId > 0);

			// use model with id
			todoNotesCreateModel = serviceResult.Model;

			// update
			var todoNotesUpdateModel = todoNotesFaker.GetOne();
			todoNotesUpdateModel.TodoNotesId = todoNotesCreateModel.TodoNotesId;
			serviceResult = todoNotesService.Update(todoNotesUpdateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// delete
			serviceResult = todoNotesService.Delete(todoNotesCreateModel);
			Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);

			// fetch

			// - make fakes
			var fakes = todoNotesFaker.GetMany(10);

			// - add fakes to database
			List<int?> ids = new List<int?>();
			foreach (var f in fakes)
			{
				serviceResult = todoNotesService.Create(f);

				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoNotesId >= 0);
				ids.Add(serviceResult.Model.TodoNotesId);
			}

			// - get fakes from database
			foreach(var id in ids)
			{
				var model = new TodoNotes()
				{
					TodoNotesId = id
				};

				serviceResult = todoNotesService.Fetch(model);
				Assert.True(serviceResult.Status == BrashActionStatus.SUCCESS, serviceResult.Message);
				Assert.True(serviceResult.Model.TodoNotesId >= 0);
			}
		}

	}
}