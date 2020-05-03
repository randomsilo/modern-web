
using Brash.Infrastructure;

namespace GoalTracker.Infrastructure.Sqlite.RepositorySql
{
	public class GoalStateRepositorySql : AAskIdRepositorySql
	{
		public GoalStateRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO GoalState (
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
				GoalStateId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalState
			WHERE
				GoalStateId = IFNULL(@GoalStateId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE GoalState
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				GoalStateId = IFNULL(@GoalStateId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM GoalState
			WHERE
				GoalStateId = IFNULL(@GoalStateId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				GoalStateId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalState
			
			";

		}
	}
}