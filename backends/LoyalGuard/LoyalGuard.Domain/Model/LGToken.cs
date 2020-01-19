
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class LGToken : IAskId, IAskIdChild
	{

		public LGToken()
		{
		}
		// IdPattern
		public int? LGTokenId { get; set; }

		// Parent IdPattern
		public int? LGAccountId { get; set; }

		// Fields
		public string Token { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? LastUsed { get; set; }
		public DateTime? Expires { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "LGTokenId";
		}
		
		public string GetParentIdPropertyName()
		{
			return "LGAccountId";
		}

	}
}