
using Brash.Infrastructure;

namespace TodoList.Infrastructure.Sqlite.RepositorySql
{
	public class ToolsRequiredRepositorySql : AAskIdRepositorySql
	{
		public ToolsRequiredRepositorySql() : base()
		{
			_sql[AskIdRepositorySqlTypes.CREATE] = @"
			INSERT INTO ToolsRequired (
				--- Columns
				TodoEntryId
				, ToolName
				, ToolWeight
			) VALUES (
				--- Values
				@TodoEntryId
				, @ToolName
				, @ToolWeight
			);
			SELECT last_insert_rowid();
			";

			_sql[AskIdRepositorySqlTypes.FETCH] = @"
			SELECT
				--- Columns
				ToolsRequiredId
				, TodoEntryId
				, ToolName
				, ToolWeight
			FROM
				ToolsRequired
			WHERE
				ToolsRequiredId = IFNULL(@ToolsRequiredId,0)
			;
			";

			_sql[AskIdRepositorySqlTypes.UPDATE] = @"
			UPDATE ToolsRequired
			SET
				TodoEntryId = @TodoEntryId
				, ToolName = @ToolName
				, ToolWeight = @ToolWeight
			WHERE
				ToolsRequiredId = IFNULL(@ToolsRequiredId,0)
			";

			_sql[AskIdRepositorySqlTypes.DELETE] = @"
			DELETE FROM ToolsRequired
			WHERE
				ToolsRequiredId = IFNULL(@ToolsRequiredId,0)
			";

			_sql[AskIdRepositorySqlTypes.FIND] = @"
			SELECT
				--- Columns
				ToolsRequiredId
				, TodoEntryId
				, ToolName
				, ToolWeight
			FROM
				ToolsRequired
			
			";

		}
	}
}