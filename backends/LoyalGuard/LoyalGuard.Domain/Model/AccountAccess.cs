
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class AccountAccess
	{

		public AccountAccess()
		{
		}
		public LGAccount Account { get; set; }
		public LGToken Token { get; set; }

    public List<string> Roles { get; set; }
		public Dictionary<string,List<string>> Privileges { get; set; }
		

	}
}