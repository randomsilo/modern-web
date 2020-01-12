
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGRightRepositorySql : AAskIdRepositorySql
	{
		public LGRightRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGRight (
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
				LGRightId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGRight
			WHERE
				LGRightId = IFNULL(@LGRightId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGRight
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				LGRightId = IFNULL(@LGRightId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGRight
			WHERE
				LGRightId = IFNULL(@LGRightId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGRightId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGRight
			
			";

		}
	}
}