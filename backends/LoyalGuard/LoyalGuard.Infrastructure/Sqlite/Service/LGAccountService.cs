
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGAccountService : ILGAccountService
	{
		protected IAskIdRepository<LGAccount> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGAccountService(IAskIdRepository<LGAccount> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGAccount> Create(LGAccount model)
		{
      if (!Utility.Hashing.isHashed(model.Password))
      {
        model.Password = Utility.Hashing.HashPassword(model.Password);
      }
			return Repository.Create(model);
		}

		public BrashActionResult<LGAccount> Fetch(LGAccount model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGAccount> Update(LGAccount model)
		{
      if (!Utility.Hashing.isHashed(model.Password))
      {
        model.Password = Utility.Hashing.HashPassword(model.Password);
      }
			return Repository.Update(model);
		}

		public BrashActionResult<LGAccount> Delete(LGAccount model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGAccount> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}