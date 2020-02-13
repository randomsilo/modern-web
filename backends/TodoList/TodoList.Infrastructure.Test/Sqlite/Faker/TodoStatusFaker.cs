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
	public class TodoStatusFaker
	{
		private Bogus.Faker<TodoStatus> _faker;
		public TodoStatusFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			int counter = 1;
			
			_faker = new Bogus.Faker<TodoStatus>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.TodoStatusId = null;
					m.ChoiceName = f.Lorem.Sentence(3);
					m.OrderNo = counter++;
					m.IsDisabled = false;
				});
		}

		public TodoStatus GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<TodoStatus> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}