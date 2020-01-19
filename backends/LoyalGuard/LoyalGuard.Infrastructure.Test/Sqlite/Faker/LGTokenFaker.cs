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
	public class LGTokenFaker
	{
		private Bogus.Faker<LGToken> _faker;
		public LGTokenFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var parentFaker = new LGAccountFaker(databaseManager, logger);
			var parent = parentFaker.GetOne();
			var parentRepository = new LGAccountRepository(databaseManager, new LGAccountRepositorySql(), logger);
			var parentAddResult = parentRepository.Create(parent);
			parent = parentAddResult.Model;
			
			_faker = new Bogus.Faker<LGToken>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.LGTokenId = null;
					m.LGAccountId = parent.LGAccountId;
					m.Token = f.Lorem.Sentence(10);
					m.Created = f.Date.Past();
					m.LastUsed = f.Date.Past();
					m.Expires = f.Date.Past();
				});
		}

		public LGToken GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<LGToken> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}