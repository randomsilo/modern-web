
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using TodoList.Domain.Model;

namespace TodoList.Domain.Service
{
	public interface ITodoEntryService 
	{
		BrashActionResult<TodoEntry> Create(TodoEntry model);
		BrashActionResult<TodoEntry> Fetch(TodoEntry model);
		BrashActionResult<TodoEntry> Update(TodoEntry model);
		BrashActionResult<TodoEntry> Delete(TodoEntry model);
		BrashQueryResult<TodoEntry> FindWhere(string where);
	}
}