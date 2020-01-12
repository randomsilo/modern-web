
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGRoleRepositorySql : AAskIdRepositorySql
	{
		public LGRoleRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGRole (
				--- Columns
				ChoiceName
				, OrderNo
				, IsDisabled
			) VALUES (
				--- Values
				@ChoiceName
				, @OrderNo
				, @IsDisabled
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				LGRoleId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGRole
			WHERE
				LGRoleId = IFNULL(@LGRoleId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGRole
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				LGRoleId = IFNULL(@LGRoleId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGRole
			WHERE
				LGRoleId = IFNULL(@LGRoleId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGRoleId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGRole
			
			";

		}
	}
}