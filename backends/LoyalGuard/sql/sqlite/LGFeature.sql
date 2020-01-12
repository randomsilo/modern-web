---
--- LoyalGuard.LGFeature
---
CREATE TABLE LGFeature (
	LGFeatureId INTEGER PRIMARY KEY AUTOINCREMENT
	, ChoiceName TEXT
	, OrderNo REAL
	, IsDisabled INTEGER
);
---
--- Choices
INSERT INTO LGFeature (ChoiceName, OrderNo) VALUES ('Feature1', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGFeature));
INSERT INTO LGFeature (ChoiceName, OrderNo) VALUES ('Feature2', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGFeature));
INSERT INTO LGFeature (ChoiceName, OrderNo) VALUES ('Feature3', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGFeature));
INSERT INTO LGFeature (ChoiceName, OrderNo) VALUES ('Feature4', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGFeature));

