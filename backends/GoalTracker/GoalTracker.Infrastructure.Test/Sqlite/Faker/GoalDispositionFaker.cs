using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Brash.Infrastructure;
using GoalTracker.Domain.Model;
using GoalTracker.Infrastructure.Sqlite.Repository;
using GoalTracker.Infrastructure.Sqlite.RepositorySql;

namespace GoalTracker.Infrastructure.Test.Sqlite.Faker
{
	public class GoalDispositionFaker
	{
		private Bogus.Faker<GoalDisposition> _faker;
		public GoalDispositionFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			int counter = 1;
			
			_faker = new Bogus.Faker<GoalDisposition>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.GoalDispositionId = null;
					m.ChoiceName = f.Lorem.Sentence(3);
					m.OrderNo = counter++;
					m.IsDisabled = false;
				});
		}

		public GoalDisposition GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<GoalDisposition> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}