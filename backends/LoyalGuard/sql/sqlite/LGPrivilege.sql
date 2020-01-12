---
--- LoyalGuard.LGPrivilege
---
CREATE TABLE LGPrivilege (
	LGPrivilegeId INTEGER PRIMARY KEY AUTOINCREMENT
	, LGAccountId INTEGER
	, Starts TIMESTAMP
	, Ends TIMESTAMP
	, FeatureIdRef INTEGER
	, RightIdRef INTEGER
	, FOREIGN KEY (LGAccountId) REFERENCES LGAccount(LGAccountId) ON DELETE CASCADE
	, FOREIGN KEY (FeatureIdRef) REFERENCES LGFeature(LGFeatureId) ON DELETE SET NULL
	, FOREIGN KEY (RightIdRef) REFERENCES LGRight(LGRightId) ON DELETE SET NULL
);
---
