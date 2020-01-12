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
	public class LGRoleFaker
	{
		private Bogus.Faker<LGRole> _faker;
		public LGRoleFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			int counter = 1;
			
			_faker = new Bogus.Faker<LGRole>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.LGRoleId = null;
					m.ChoiceName = f.Lorem.Sentence(3);
					m.OrderNo = counter++;
					m.IsDisabled = false;
				});
		}

		public LGRole GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<LGRole> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}