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
	, PasswordConfirmation TEXT
	, PasswordHashed TEXT
	, RoleIdRef INTEGER
	, FOREIGN KEY (RoleIdRef) REFERENCES LGRole(LGRoleId) ON DELETE SET NULL
);
---
