
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
				, AbilityIdRef
			) VALUES (
				--- Values
				@LGAccountId
				, @Starts
				, @Ends
				, @FeatureIdRef
				, @AbilityIdRef
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
				, AbilityIdRef
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
				, AbilityIdRef = @AbilityIdRef
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
				, AbilityIdRef
			FROM
				LGPrivilege
			
			";

		}
	}
}