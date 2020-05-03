
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace GoalTracker.Domain.Model
{
	public class Goal : IAskId
	{

		public Goal()
		{
		}
		// IdPattern
		public int? GoalId { get; set; }

		// Fields
		public string Description { get; set; }
		public string Notes { get; set; }

		// References
		public int? DispositionIdRef { get; set; }
		public int? StateIdRef { get; set; }
		public int? StatusIdRef { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "GoalId";
		}

	}
}