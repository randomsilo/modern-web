
using Brash.Infrastructure;

namespace GoalTracker.Infrastructure.Sqlite.RepositorySql
{
	public class GoalStatusRepositorySql : AAskIdRepositorySql
	{
		public GoalStatusRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO GoalStatus (
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
				GoalStatusId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalStatus
			WHERE
				GoalStatusId = IFNULL(@GoalStatusId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE GoalStatus
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				GoalStatusId = IFNULL(@GoalStatusId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM GoalStatus
			WHERE
				GoalStatusId = IFNULL(@GoalStatusId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				GoalStatusId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalStatus
			
			";

		}
	}
}