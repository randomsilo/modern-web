---
--- GoalTracker.GoalStatus
---
CREATE TABLE GoalStatus (
	GoalStatusId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO GoalStatus (ChoiceName, OrderNo) VALUES ('1', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalStatus));
INSERT INTO GoalStatus (ChoiceName, OrderNo) VALUES ('2', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalStatus));
INSERT INTO GoalStatus (ChoiceName, OrderNo) VALUES ('3', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM GoalStatus));

