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
	public class LGPrivilegeFaker
	{
		private Bogus.Faker<LGPrivilege> _faker;
		public LGPrivilegeFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var parentFaker = new LGAccountFaker(databaseManager, logger);
			var parent = parentFaker.GetOne();
			var parentRepository = new LGAccountRepository(databaseManager, new LGAccountRepositorySql(), logger);
			var parentAddResult = parentRepository.Create(parent);
			parent = parentAddResult.Model;
			
			var featureFaker = new LGFeatureFaker(databaseManager, logger);
			var featureFake = featureFaker.GetOne();
			var featureRepository = new LGFeatureRepository(databaseManager, new LGFeatureRepositorySql(), logger);
			var featureFakeResult = featureRepository.Create(featureFake);
			featureFake = featureFakeResult.Model;
			
			var rightFaker = new LGRightFaker(databaseManager, logger);
			var rightFake = rightFaker.GetOne();
			var rightRepository = new LGRightRepository(databaseManager, new LGRightRepositorySql(), logger);
			var rightFakeResult = rightRepository.Create(rightFake);
			rightFake = rightFakeResult.Model;
			
			_faker = new Bogus.Faker<LGPrivilege>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.LGPrivilegeId = null;
					m.LGAccountId = parent.LGAccountId;
					m.Starts = f.Date.Past();
					m.Ends = f.Date.Past();
					m.FeatureIdRef = featureFake.LGFeatureId;
					m.RightIdRef = rightFake.LGRightId;
				});
		}

		public LGPrivilege GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<LGPrivilege> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}