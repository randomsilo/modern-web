using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Brash.Infrastructure;
using TodoList.Domain.Model;
using TodoList.Infrastructure.Sqlite.Repository;
using TodoList.Infrastructure.Sqlite.RepositorySql;

namespace TodoList.Infrastructure.Test.Sqlite.Faker
{
	public class TodoEntryFaker
	{
		private Bogus.Faker<TodoEntry> _faker;
		public TodoEntryFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var entryStatusFaker = new TodoStatusFaker(databaseManager, logger);
			var entryStatusFake = entryStatusFaker.GetOne();
			var entryStatusRepository = new TodoStatusRepository(databaseManager, new TodoStatusRepositorySql(), logger);
			var entryStatusFakeResult = entryStatusRepository.Create(entryStatusFake);
			entryStatusFake = entryStatusFakeResult.Model;
			
			_faker = new Bogus.Faker<TodoEntry>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.TodoEntryId = null;
					m.Summary = f.Lorem.Sentence(10);
					m.Details = f.Lorem.Sentence(10);
					m.DueDate = f.Date.Past();
					m.EntryStatusIdRef = entryStatusFake.TodoStatusId;
				});
		}

		public TodoEntry GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<TodoEntry> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}