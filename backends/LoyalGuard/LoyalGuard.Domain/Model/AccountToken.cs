
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class AccountToken
	{

		public AccountToken()
		{
		}
		public LGAccount Account { get; set; }
		public LGToken Token { get; set; }
		

	}
}