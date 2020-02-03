
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGAccountRepositorySql : AAskIdRepositorySql
	{
		public LGAccountRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGAccount (
				--- Columns
				LastName
				, FirstName
				, MiddleName
				, UserName
				, Email
				, Password
				, RoleIdRef
			) VALUES (
				--- Values
				@LastName
				, @FirstName
				, @MiddleName
				, @UserName
				, @Email
				, @Password
				, @RoleIdRef
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				LGAccountId
				, LastName
				, FirstName
				, MiddleName
				, UserName
				, Email
				, Password
				, RoleIdRef
			FROM
				LGAccount
			WHERE
				LGAccountId = IFNULL(@LGAccountId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGAccount
			SET
				LastName = @LastName
				, FirstName = @FirstName
				, MiddleName = @MiddleName
				, UserName = @UserName
				, Email = @Email
				, Password = @Password
				, RoleIdRef = @RoleIdRef
			WHERE
				LGAccountId = IFNULL(@LGAccountId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGAccount
			WHERE
				LGAccountId = IFNULL(@LGAccountId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGAccountId
				, LastName
				, FirstName
				, MiddleName
				, UserName
				, Email
				, Password
				, RoleIdRef
			FROM
				LGAccount
			
			";

		}
	}
}