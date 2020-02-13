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
