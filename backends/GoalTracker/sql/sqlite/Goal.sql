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
