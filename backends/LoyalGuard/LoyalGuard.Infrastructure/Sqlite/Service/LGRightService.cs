
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGRightService : ILGRightService
	{
		protected IAskIdRepository<LGRight> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGRightService(IAskIdRepository<LGRight> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGRight> Create(LGRight model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGRight> Fetch(LGRight model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGRight> Update(LGRight model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGRight> Delete(LGRight model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGRight> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}