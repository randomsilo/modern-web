---
--- LoyalGuard.LGRole
---
CREATE TABLE LGRole (
	LGRoleId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO LGRole (ChoiceName, OrderNo) VALUES ('Unknown', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRole));
INSERT INTO LGRole (ChoiceName, OrderNo) VALUES ('User', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRole));
INSERT INTO LGRole (ChoiceName, OrderNo) VALUES ('Auditor', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRole));
INSERT INTO LGRole (ChoiceName, OrderNo) VALUES ('Administrator', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRole));

