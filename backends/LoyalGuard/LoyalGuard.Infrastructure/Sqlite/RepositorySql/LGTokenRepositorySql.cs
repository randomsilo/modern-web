
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGTokenRepositorySql : AAskIdRepositorySql
	{
		public LGTokenRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGToken (
				--- Columns
				LGAccountId
				, Token
				, Created
				, LastUsed
				, Expires
			) VALUES (
				--- Values
				@LGAccountId
				, @Token
				, @Created
				, @LastUsed
				, @Expires
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				LGTokenId
				, LGAccountId
				, Token
				, datetime(Created,'unixepoch') AS Created
				, datetime(LastUsed,'unixepoch') AS LastUsed
				, datetime(Expires,'unixepoch') AS Expires
			FROM
				LGToken
			WHERE
				LGTokenId = IFNULL(@LGTokenId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGToken
			SET
				LGAccountId = @LGAccountId
				, Token = @Token
				, Created = @Created
				, LastUsed = @LastUsed
				, Expires = @Expires
			WHERE
				LGTokenId = IFNULL(@LGTokenId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGToken
			WHERE
				LGTokenId = IFNULL(@LGTokenId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGTokenId
				, LGAccountId
				, Token
				, datetime(Created,'unixepoch') AS Created
				, datetime(LastUsed,'unixepoch') AS LastUsed
				, datetime(Expires,'unixepoch') AS Expires
			FROM
				LGToken
			
			";

		}
	}
}