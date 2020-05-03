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
	public class GoalFaker
	{
		private Bogus.Faker<Goal> _faker;
		public GoalFaker(IManageDatabase databaseManager, ILogger logger)
		{
			var random = new Random();
			int randomNumber = random.Next();
			Bogus.Randomizer.Seed = new Random(randomNumber);
			
			var dispositionFaker = new GoalDispositionFaker(databaseManager, logger);
			var dispositionFake = dispositionFaker.GetOne();
			var dispositionRepository = new GoalDispositionRepository(databaseManager, new GoalDispositionRepositorySql(), logger);
			var dispositionFakeResult = dispositionRepository.Create(dispositionFake);
			dispositionFake = dispositionFakeResult.Model;
			
			var stateFaker = new GoalStateFaker(databaseManager, logger);
			var stateFake = stateFaker.GetOne();
			var stateRepository = new GoalStateRepository(databaseManager, new GoalStateRepositorySql(), logger);
			var stateFakeResult = stateRepository.Create(stateFake);
			stateFake = stateFakeResult.Model;
			
			var statusFaker = new GoalStatusFaker(databaseManager, logger);
			var statusFake = statusFaker.GetOne();
			var statusRepository = new GoalStatusRepository(databaseManager, new GoalStatusRepositorySql(), logger);
			var statusFakeResult = statusRepository.Create(statusFake);
			statusFake = statusFakeResult.Model;
			
			_faker = new Bogus.Faker<Goal>()
				.StrictMode(false)
				.Rules((f, m) =>
				{
					m.GoalId = null;
					m.Description = f.Lorem.Sentence(10);
					m.Notes = f.Lorem.Sentence(10);
					m.DispositionIdRef = dispositionFake.GoalDispositionId;
					m.StateIdRef = stateFake.GoalStateId;
					m.StatusIdRef = statusFake.GoalStatusId;
				});
		}

		public Goal GetOne()
		{
			return _faker.Generate(1).First();
		}
		
		public List<Goal> GetMany(int count)
		{
			return _faker.Generate(count);
		}
	}
}