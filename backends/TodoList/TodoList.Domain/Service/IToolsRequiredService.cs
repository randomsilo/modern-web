
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brash.Infrastructure;
using Brash.Model;
using TodoList.Domain.Model;

namespace TodoList.Domain.Service
{
	public interface IToolsRequiredService 
	{
		BrashActionResult<ToolsRequired> Create(ToolsRequired model);
		BrashActionResult<ToolsRequired> Fetch(ToolsRequired model);
		BrashActionResult<ToolsRequired> Update(ToolsRequired model);
		BrashActionResult<ToolsRequired> Delete(ToolsRequired model);
		BrashQueryResult<ToolsRequired> FindWhere(string where);
		BrashQueryResult<ToolsRequired> FindByParent(int id);
	}
}