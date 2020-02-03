
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGAbilityService : ILGAbilityService
	{
		protected IAskIdRepository<LGAbility> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGAbilityService(IAskIdRepository<LGAbility> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGAbility> Create(LGAbility model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGAbility> Fetch(LGAbility model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGAbility> Update(LGAbility model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGAbility> Delete(LGAbility model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGAbility> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}