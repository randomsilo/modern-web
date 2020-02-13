
using Brash.Infrastructure;

namespace TodoList.Infrastructure.Sqlite.RepositorySql
{
	public class TodoEntryRepositorySql : AAskIdRepositorySql
	{
		public TodoEntryRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO TodoEntry (
				--- Columns
				Summary
				, Details
				, DueDate
				, EntryStatusIdRef
			) VALUES (
				--- Values
				@Summary
				, @Details
				, @DueDate
				, @EntryStatusIdRef
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				TodoEntryId
				, Summary
				, Details
				, datetime(DueDate,'unixepoch') AS DueDate
				, EntryStatusIdRef
			FROM
				TodoEntry
			WHERE
				TodoEntryId = IFNULL(@TodoEntryId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE TodoEntry
			SET
				Summary = @Summary
				, Details = @Details
				, DueDate = @DueDate
				, EntryStatusIdRef = @EntryStatusIdRef
			WHERE
				TodoEntryId = IFNULL(@TodoEntryId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM TodoEntry
			WHERE
				TodoEntryId = IFNULL(@TodoEntryId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				TodoEntryId
				, Summary
				, Details
				, datetime(DueDate,'unixepoch') AS DueDate
				, EntryStatusIdRef
			FROM
				TodoEntry
			
			";

		}
	}
}