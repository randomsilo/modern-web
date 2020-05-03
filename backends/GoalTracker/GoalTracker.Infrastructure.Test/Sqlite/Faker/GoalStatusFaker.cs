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
	public class GoalStatusFaker
	{
		private Bogus.Faker<GoalStatus> _faker;
		public GoalStatusFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			int counter = 1;
			
			_faker = new Bogus.Faker<GoalStatus>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.GoalStatusId = null;
					m.ChoiceName = f.Lorem.Sentence(3);
					m.OrderNo = counter++;
					m.IsDisabled = false;
				});
		}

		public GoalStatus GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<GoalStatus> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}