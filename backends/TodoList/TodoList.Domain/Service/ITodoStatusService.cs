
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using TodoList.Domain.Model;

namespace TodoList.Domain.Service
{
	public interface ITodoStatusService 
	{
		BrashActionResult<TodoStatus> Create(TodoStatus model);
		BrashActionResult<TodoStatus> Fetch(TodoStatus model);
		BrashActionResult<TodoStatus> Update(TodoStatus model);
		BrashActionResult<TodoStatus> Delete(TodoStatus model);
		BrashQueryResult<TodoStatus> FindWhere(string where);
	}
}