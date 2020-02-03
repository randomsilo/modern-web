
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class LGAccount : IAskId
	{

		public LGAccount()
		{
		}
		// IdPattern
		public int? LGAccountId { get; set; }

		// Fields
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		// References
		public int? RoleIdRef { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "LGAccountId";
		}

	}
}