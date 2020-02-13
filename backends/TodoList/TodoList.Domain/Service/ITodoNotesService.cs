
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using TodoList.Domain.Model;

namespace TodoList.Domain.Service
{
	public interface ITodoNotesService 
	{
		BrashActionResult<TodoNotes> Create(TodoNotes model);
		BrashActionResult<TodoNotes> Fetch(TodoNotes model);
		BrashActionResult<TodoNotes> Update(TodoNotes model);
		BrashActionResult<TodoNotes> Delete(TodoNotes model);
		BrashQueryResult<TodoNotes> FindWhere(string where);
		BrashQueryResult<TodoNotes> FindByParent(int id);
	}
}