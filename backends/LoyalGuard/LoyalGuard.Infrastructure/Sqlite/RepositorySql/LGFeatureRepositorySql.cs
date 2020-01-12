
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGFeatureRepositorySql : AAskIdRepositorySql
	{
		public LGFeatureRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGFeature (
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
				LGFeatureId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGFeature
			WHERE
				LGFeatureId = IFNULL(@LGFeatureId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGFeature
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				LGFeatureId = IFNULL(@LGFeatureId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGFeature
			WHERE
				LGFeatureId = IFNULL(@LGFeatureId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGFeatureId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGFeature
			
			";

		}
	}
}