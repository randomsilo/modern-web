---
--- GoalTracker.GoalDisposition
---
CREATE TABLE GoalDisposition (
	GoalDispositionId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO GoalDisposition (ChoiceName, OrderNo) VALUES ('A', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalDisposition));
INSERT INTO GoalDisposition (ChoiceName, OrderNo) VALUES ('B', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalDisposition));
INSERT INTO GoalDisposition (ChoiceName, OrderNo) VALUES ('C', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalDisposition));

