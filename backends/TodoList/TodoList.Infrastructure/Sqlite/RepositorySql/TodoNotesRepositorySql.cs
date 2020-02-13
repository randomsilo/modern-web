
using Brash.Infrastructure;

namespace TodoList.Infrastructure.Sqlite.RepositorySql
{
	public class TodoNotesRepositorySql : AAskIdRepositorySql
	{
		public TodoNotesRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO TodoNotes (
				--- Columns
				TodoEntryId
				, Note
			) VALUES (
				--- Values
				@TodoEntryId
				, @Note
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				TodoNotesId
				, TodoEntryId
				, Note
			FROM
				TodoNotes
			WHERE
				TodoNotesId = IFNULL(@TodoNotesId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE TodoNotes
			SET
				TodoEntryId = @TodoEntryId
				, Note = @Note
			WHERE
				TodoNotesId = IFNULL(@TodoNotesId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM TodoNotes
			WHERE
				TodoNotesId = IFNULL(@TodoNotesId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				TodoNotesId
				, TodoEntryId
				, Note
			FROM
				TodoNotes
			
			";

		}
	}
}