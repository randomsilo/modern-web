using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Brash.Infrastructure;
using LoyalGuard.Domain.Model;
using LoyalGuard.Infrastructure.Sqlite.Repository;
using LoyalGuard.Infrastructure.Sqlite.RepositorySql;

namespace LoyalGuard.Infrastructure.Test.Sqlite.Faker
{
	public class LGFeatureFaker
	{
		private Bogus.Faker<LGFeature> _faker;
		public LGFeatureFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			int counter = 1;
			
			_faker = new Bogus.Faker<LGFeature>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.LGFeatureId = null;
					m.ChoiceName = f.Lorem.Sentence(3);
					m.OrderNo = counter++;
					m.IsDisabled = false;
				});
		}

		public LGFeature GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<LGFeature> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}