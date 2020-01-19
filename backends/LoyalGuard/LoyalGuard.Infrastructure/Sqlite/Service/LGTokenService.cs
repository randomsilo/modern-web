
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGTokenService : ILGTokenService
	{
		protected IAskIdRepository<LGToken> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGTokenService(IAskIdRepository<LGToken> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGToken> Create(LGToken model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGToken> Fetch(LGToken model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGToken> Update(LGToken model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGToken> Delete(LGToken model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGToken> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}

		public BrashQueryResult<LGToken> FindByParent(int id)
		{
			string where = $"WHERE LGAccountId = {id}";
			return Repository.FindWhere(where);
		}
	}
}