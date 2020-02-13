---
--- TodoList.ToolsRequired
---
CREATE TABLE ToolsRequired (
	ToolsRequiredId INTEGER PRIMARY KEY AUTOINCREMENT
	, TodoEntryId INTEGER
	, ToolName TEXT
	, ToolWeight NUMERIC
	, FOREIGN KEY (TodoEntryId) REFERENCES TodoEntry(TodoEntryId) ON DELETE CASCADE
);
---
