---
--- LoyalGuard.LGAbility
---
CREATE TABLE LGAbility (
	LGAbilityId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('None', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('View', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Edit', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Delete', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Run', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Import', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Export', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));

