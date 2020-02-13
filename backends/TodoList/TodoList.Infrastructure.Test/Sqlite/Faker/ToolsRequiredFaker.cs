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
	public class ToolsRequiredFaker
	{
		private Bogus.Faker<ToolsRequired> _faker;
		public ToolsRequiredFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var parentFaker = new TodoEntryFaker(databaseManager, logger);
			var parent = parentFaker.GetOne();
			var parentRepository = new TodoEntryRepository(databaseManager, new TodoEntryRepositorySql(), logger);
			var parentAddResult = parentRepository.Create(parent);
			parent = parentAddResult.Model;
			
			_faker = new Bogus.Faker<ToolsRequired>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.ToolsRequiredId = null;
					m.TodoEntryId = parent.TodoEntryId;
					m.ToolName = f.Lorem.Sentence(10);
					m.ToolWeight = f.Random.Decimal();
				});
		}

		public ToolsRequired GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<ToolsRequired> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}