
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace TodoList.Domain.Model
{
	public class TodoEntry : IAskId
	{

		public TodoEntry()
		{
		}
		// IdPattern
		public int? TodoEntryId { get; set; }

		// Fields
		public string Summary { get; set; }
		public string Details { get; set; }
		public DateTime? DueDate { get; set; }

		// References
		public int? EntryStatusIdRef { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "TodoEntryId";
		}

	}
}