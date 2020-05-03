---
--- GoalTracker.GoalState
---
CREATE TABLE GoalState (
	GoalStateId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO GoalState (ChoiceName, OrderNo) VALUES ('X', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalState));
INSERT INTO GoalState (ChoiceName, OrderNo) VALUES ('Y', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalState));
INSERT INTO GoalState (ChoiceName, OrderNo) VALUES ('Z', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalState));

