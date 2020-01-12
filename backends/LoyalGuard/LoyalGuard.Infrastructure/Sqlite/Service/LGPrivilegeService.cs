
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class LGPrivilegeService : ILGPrivilegeService
	{
		protected IAskIdRepository<LGPrivilege> Repository
		{ 
			get; private set;
		}
		protected ILogger Logger { get; set; }

		public LGPrivilegeService(IAskIdRepository<LGPrivilege> repository, ILogger logger)
		{
			Repository = repository;
			Logger = logger;
		}

		public BrashActionResult<LGPrivilege> Create(LGPrivilege model)
		{
			return Repository.Create(model);
		}

		public BrashActionResult<LGPrivilege> Fetch(LGPrivilege model)
		{
			return Repository.Fetch(model);
		}

		public BrashActionResult<LGPrivilege> Update(LGPrivilege model)
		{
			return Repository.Update(model);
		}

		public BrashActionResult<LGPrivilege> Delete(LGPrivilege model)
		{
			return Repository.Delete(model);
		}

		public BrashQueryResult<LGPrivilege> FindWhere(string where)
		{
			return Repository.FindWhere(where);
		}

		public BrashQueryResult<LGPrivilege> FindByParent(int id)
		{
			string where = $"WHERE LGAccountId = {id}";
			return Repository.FindWhere(where);
		}
	}
}