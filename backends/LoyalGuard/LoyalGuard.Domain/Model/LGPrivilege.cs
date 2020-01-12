
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class LGPrivilege : IAskId, IAskIdChild
	{

		public LGPrivilege()
		{
		}
		// IdPattern
		public int? LGPrivilegeId { get; set; }

		// Parent IdPattern
		public int? LGAccountId { get; set; }

		// Fields
		public DateTime? Starts { get; set; }
		public DateTime? Ends { get; set; }

		// References
		public int? FeatureIdRef { get; set; }
		public int? RightIdRef { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "LGPrivilegeId";
		}
		
		public string GetParentIdPropertyName()
		{
			return "LGAccountId";
		}

	}
}