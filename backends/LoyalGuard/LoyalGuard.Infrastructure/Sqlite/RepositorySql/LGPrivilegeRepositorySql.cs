
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGPrivilegeRepositorySql : AAskIdRepositorySql
	{
		public LGPrivilegeRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGPrivilege (
				--- Columns
				LGAccountId
				, Starts
				, Ends
				, FeatureIdRef
				, RightIdRef
			) VALUES (
				--- Values
				@LGAccountId
				, @Starts
				, @Ends
				, @FeatureIdRef
				, @RightIdRef
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				LGPrivilegeId
				, LGAccountId
				, datetime(Starts,'unixepoch') AS Starts
				, datetime(Ends,'unixepoch') AS Ends
				, FeatureIdRef
				, RightIdRef
			FROM
				LGPrivilege
			WHERE
				LGPrivilegeId = IFNULL(@LGPrivilegeId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGPrivilege
			SET
				LGAccountId = @LGAccountId
				, Starts = @Starts
				, Ends = @Ends
				, FeatureIdRef = @FeatureIdRef
				, RightIdRef = @RightIdRef
			WHERE
				LGPrivilegeId = IFNULL(@LGPrivilegeId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGPrivilege
			WHERE
				LGPrivilegeId = IFNULL(@LGPrivilegeId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGPrivilegeId
				, LGAccountId
				, datetime(Starts,'unixepoch') AS Starts
				, datetime(Ends,'unixepoch') AS Ends
				, FeatureIdRef
				, RightIdRef
			FROM
				LGPrivilege
			
			";

		}
	}
}