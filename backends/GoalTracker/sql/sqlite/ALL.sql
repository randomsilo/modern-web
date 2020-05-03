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

---
--- GoalTracker.Goal
---
CREATE TABLE Goal (
	GoalId INTEGER PRIMARY KEY AUTOINCREMENT
	, Description TEXT
	, Notes TEXT
	, DispositionIdRef INTEGER
	, StateIdRef INTEGER
	, StatusIdRef INTEGER
	, FOREIGN KEY (DispositionIdRef) REFERENCES GoalDisposition(GoalDispositionId) ON DELETE SET NULL
	, FOREIGN KEY (StateIdRef) REFERENCES GoalState(GoalStateId) ON DELETE SET NULL
	, FOREIGN KEY (StatusIdRef) REFERENCES GoalStatus(GoalStatusId) ON DELETE SET NULL
);
---
