
using Brash.Infrastructure;

namespace LoyalGuard.Infrastructure.Sqlite.RepositorySql
{
	public class LGAbilityRepositorySql : AAskIdRepositorySql
	{
		public LGAbilityRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO LGAbility (
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
				LGAbilityId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGAbility
			WHERE
				LGAbilityId = IFNULL(@LGAbilityId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE LGAbility
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				LGAbilityId = IFNULL(@LGAbilityId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM LGAbility
			WHERE
				LGAbilityId = IFNULL(@LGAbilityId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				LGAbilityId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				LGAbility
			
			";

		}
	}
}