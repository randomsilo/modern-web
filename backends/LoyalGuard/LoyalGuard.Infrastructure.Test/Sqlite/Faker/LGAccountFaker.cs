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
	public class LGAccountFaker
	{
		private Bogus.Faker<LGAccount> _faker;
		public LGAccountFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var roleFaker = new LGRoleFaker(databaseManager, logger);
			var roleFake = roleFaker.GetOne();
			var roleRepository = new LGRoleRepository(databaseManager, new LGRoleRepositorySql(), logger);
			var roleFakeResult = roleRepository.Create(roleFake);
			roleFake = roleFakeResult.Model;
			
			_faker = new Bogus.Faker<LGAccount>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.LGAccountId = null;
					m.LastName = f.Name.LastName(0);
					m.FirstName = f.Name.FirstName(0);
					m.MiddleName = f.Name.FirstName(0);
					m.UserName = f.Internet.UserName(m.FirstName, m.LastName);
					m.Email = f.Internet.Email(m.FirstName, m.LastName);
					m.Password = f.Lorem.Sentence(10);
					m.PasswordConfirmation = f.Lorem.Sentence(10);
					m.PasswordHashed = f.Lorem.Sentence(10);
					m.RoleIdRef = roleFake.LGRoleId;
				});
		}

		public LGAccount GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<LGAccount> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}