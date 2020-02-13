
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace TodoList.Domain.Model
{
	public class TodoStatus : IAskId
	{

		public TodoStatus()
		{
		}
		// IdPattern
		public int? TodoStatusId { get; set; }

		// Additional Patterns
		public string ChoiceName { get; set; }
		public decimal? OrderNo { get; set; }
		public bool? IsDisabled { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "TodoStatusId";
		}

	}
}