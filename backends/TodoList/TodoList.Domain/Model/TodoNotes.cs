
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace TodoList.Domain.Model
{
	public class TodoNotes : IAskId, IAskIdChild
	{

		public TodoNotes()
		{
		}
		// IdPattern
		public int? TodoNotesId { get; set; }

		// Parent IdPattern
		public int? TodoEntryId { get; set; }

		// Fields
		public string Note { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "TodoNotesId";
		}
		
		public string GetParentIdPropertyName()
		{
			return "TodoEntryId";
		}

	}
}