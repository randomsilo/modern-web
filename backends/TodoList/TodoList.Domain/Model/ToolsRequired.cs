
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Model;

namespace TodoList.Domain.Model
{
	public class ToolsRequired : IAskId, IAskIdChild
	{

		public ToolsRequired()
		{
		}
		// IdPattern
		public int? ToolsRequiredId { get; set; }

		// Parent IdPattern
		public int? TodoEntryId { get; set; }

		// Fields
		public string ToolName { get; set; }
		public decimal? ToolWeight { get; set; }


		// Interface Implementations
		public string GetIdPropertyName()
		{
			return "ToolsRequiredId";
		}
		
		public string GetParentIdPropertyName()
		{
			return "TodoEntryId";
		}

	}
}