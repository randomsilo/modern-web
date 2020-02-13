
using Brash.Infrastructure;

namespace TodoList.Infrastructure.Sqlite.RepositorySql
{
	public class TodoStatusRepositorySql : AAskIdRepositorySql
	{
		public TodoStatusRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO TodoStatus (
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
				TodoStatusId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				TodoStatus
			WHERE
				TodoStatusId = IFNULL(@TodoStatusId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE TodoStatus
			SET
				ChoiceName = @ChoiceName
				, OrderNo = @OrderNo
				, IsDisabled = @IsDisabled
			WHERE
				TodoStatusId = IFNULL(@TodoStatusId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM TodoStatus
			WHERE
				TodoStatusId = IFNULL(@TodoStatusId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				TodoStatusId
				, ChoiceName
				, OrderNo
				, IsDisabled
			FROM
				TodoStatus
			
			";

		}
	}
}