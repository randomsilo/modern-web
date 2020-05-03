
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace GoalTracker.Domain.Model
{
	public class GoalState : IAskId
	{

		public GoalState()
		{
		}
		// IdPattern
		public int? GoalStateId { get; set; }

		// Additional Patterns
		public string ChoiceName { get; set; }
		public decimal? OrderNo { get; set; }
		public bool? IsDisabled { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "GoalStateId";
		}

	}
}