
using Brash.Infrastructure;

namespace GoalTracker.Infrastructure.Sqlite.RepositorySql
{
	public class GoalDispositionRepositorySql : AAskIdRepositorySql
	{
		public GoalDispositionRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO GoalDisposition (
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
				GoalDispositionId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalDisposition
			WHERE
				GoalDispositionId = IFNULL(@GoalDispositionId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE GoalDisposition
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				GoalDispositionId = IFNULL(@GoalDispositionId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM GoalDisposition
			WHERE
				GoalDispositionId = IFNULL(@GoalDispositionId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				GoalDispositionId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				GoalDisposition
			
			";

		}
	}
}