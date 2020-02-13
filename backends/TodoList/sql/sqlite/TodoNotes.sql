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
