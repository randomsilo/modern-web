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
	public class TodoNotesFaker
	{
		private Bogus.Faker<TodoNotes> _faker;
		public TodoNotesFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var parentFaker = new TodoEntryFaker(databaseManager, logger);
			var parent = parentFaker.GetOne();
			var parentRepository = new TodoEntryRepository(databaseManager, new TodoEntryRepositorySql(), logger);
			var parentAddResult = parentRepository.Create(parent);
			parent = parentAddResult.Model;
			
			_faker = new Bogus.Faker<TodoNotes>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.TodoNotesId = null;
					m.TodoEntryId = parent.TodoEntryId;
					m.Note = f.Lorem.Sentence(10);
				});
		}

		public TodoNotes GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<TodoNotes> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}