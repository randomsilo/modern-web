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
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('View', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Edit', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Delete', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Run', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Import', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));
INSERT INTO LGAbility (ChoiceName, OrderNo) VALUES ('Export', (SELECT IFNULL(MAX(OrderNo),0)+1 FROM LGAbility));

---
--- LoyalGuard.LGAccount
---
CREATE TABLE LGAccount (
	LGAccountId INTEGER PRIMARY KEY AUTOINCREMENT
	, LastName TEXT
	, FirstName TEXT
	, MiddleName TEXT
	, UserName TEXT
	, Email TEXT
	, Password TEXT
	, RoleIdRef INTEGER
	, FOREIGN KEY (RoleIdRef) REFERENCES LGRole(LGRoleId) ON DELETE SET NULL
);
---
---
--- LoyalGuard.LGPrivilege
---
CREATE TABLE LGPrivilege (
	LGPrivilegeId INTEGER PRIMARY KEY AUTOINCREMENT
	, LGAccountId INTEGER
	, Starts TIMESTAMP
	, Ends TIMESTAMP
	, FeatureIdRef INTEGER
	, AbilityIdRef INTEGER
	, FOREIGN KEY (LGAccountId) REFERENCES LGAccount(LGAccountId) ON DELETE CASCADE
	, FOREIGN KEY (FeatureIdRef) REFERENCES LGFeature(LGFeatureId) ON DELETE SET NULL
	, FOREIGN KEY (AbilityIdRef) REFERENCES LGAbility(LGAbilityId) ON DELETE SET NULL
);
---
---
--- LoyalGuard.LGToken
---
CREATE TABLE LGToken (
	LGTokenId INTEGER PRIMARY KEY AUTOINCREMENT
	, LGAccountId INTEGER
	, Token TEXT
	, Created TIMESTAMP
	, LastUsed TIMESTAMP
	, Expires TIMESTAMP
	, FOREIGN KEY (LGAccountId) REFERENCES LGAccount(LGAccountId) ON DELETE CASCADE
);
---
