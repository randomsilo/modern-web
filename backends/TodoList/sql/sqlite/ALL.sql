---
--- TodoList.TodoStatus
---
CREATE TABLE TodoStatus (
	TodoStatusId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO TodoStatus (ChoiceName, OrderNo) VALUES ('New', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM TodoStatus));
INSERT INTO TodoStatus (ChoiceName, OrderNo) VALUES ('In Progress', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM TodoStatus));
INSERT INTO TodoStatus (ChoiceName, OrderNo) VALUES ('Done', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM TodoStatus));

---
--- TodoList.TodoEntry
---
CREATE TABLE TodoEntry (
	TodoEntryId INTEGER PRIMARY KEY AUTOINCREMENT
	, Summary TEXT
	, Details TEXT
	, DueDate TIMESTAMP
	, EntryStatusIdRef INTEGER
	, FOREIGN KEY (EntryStatusIdRef) REFERENCES TodoStatus(TodoStatusId) ON DELETE SET NULL
);
---
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
---
--- TodoList.TodoNotes
---
CREATE TABLE TodoNotes (
	TodoNotesId INTEGER PRIMARY KEY AUTOINCREMENT
	, TodoEntryId INTEGER
	, Note TEXT
	, FOREIGN KEY (TodoEntryId) REFERENCES TodoEntry(TodoEntryId) ON DELETE CASCADE
);
---
