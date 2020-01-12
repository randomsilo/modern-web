
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGRoleService : ILGRoleService
	{
		protected IAskIdRepository<LGRole> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGRoleService(IAskIdRepository<LGRole> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGRole> Create(LGRole model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGRole> Fetch(LGRole model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGRole> Update(LGRole model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGRole> Delete(LGRole model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGRole> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}