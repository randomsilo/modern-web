
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace LoyalGuard.Domain.Model
{
	public class LGFeature : IAskId
	{

		public LGFeature()
		{
		}
		// IdPattern
		public int? LGFeatureId { get; set; }

		// Additional Patterns
		public string ChoiceName { get; set; }
		public decimal? OrderNo { get; set; }
		public bool? IsDisabled { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "LGFeatureId";
		}

	}
}