---
--- LoyalGuard.LGRight
---
CREATE TABLE LGRight (
	LGRightId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('None', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('View', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('Edit', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('View, Run', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('Edit, Run', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('All', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));
INSERT INTO LGRight (ChoiceName, OrderNo) VALUES ('Other', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGRight));

