
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace GoalTracker.Domain.Model
{
	public class GoalDisposition : IAskId
	{

		public GoalDisposition()
		{
		}
		// IdPattern
		public int? GoalDispositionId { get; set; }

		// Additional Patterns
		public string ChoiceName { get; set; }
		public decimal? OrderNo { get; set; }
		public bool? IsDisabled { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "GoalDispositionId";
		}

	}
}