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

