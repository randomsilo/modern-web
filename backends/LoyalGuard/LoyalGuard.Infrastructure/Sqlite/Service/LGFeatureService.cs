
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGFeatureService : ILGFeatureService
	{
		protected IAskIdRepository<LGFeature> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGFeatureService(IAskIdRepository<LGFeature> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGFeature> Create(LGFeature model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGFeature> Fetch(LGFeature model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGFeature> Update(LGFeature model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGFeature> Delete(LGFeature model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGFeature> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}
	}
}