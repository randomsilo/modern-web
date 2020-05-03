
using Brash.Infrastructure;

namespace GoalTracker.Infrastructure.Sqlite.RepositorySql
{
	public class GoalRepositorySql : AAskIdRepositorySql
	{
		public GoalRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO Goal (
				--- Columns
				Description
				, Notes
				, DispositionIdRef
				, StateIdRef
				, StatusIdRef
			) VALUES (
				--- Values
				@Description
				, @Notes
				, @DispositionIdRef
				, @StateIdRef
				, @StatusIdRef
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				GoalId
				, Description
				, Notes
				, DispositionIdRef
				, StateIdRef
				, StatusIdRef
			FROM
				Goal
			WHERE
				GoalId = IFNULL(@GoalId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE Goal
			SET
				Description = @Description
				, Notes = @Notes
				, DispositionIdRef = @DispositionIdRef
				, StateIdRef = @StateIdRef
				, StatusIdRef = @StatusIdRef
			WHERE
				GoalId = IFNULL(@GoalId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM Goal
			WHERE
				GoalId = IFNULL(@GoalId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				GoalId
				, Description
				, Notes
				, DispositionIdRef
				, StateIdRef
				, StatusIdRef
			FROM
				Goal
			
			";

		}
	}
}