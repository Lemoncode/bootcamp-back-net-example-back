using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lemoncode.LibraryExample.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class AddedTriggerToUpdateAverageInBooks : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			var sql = @"
CREATE TRIGGER trg_UpdateBookAverage ON Reviews
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tabla temporal para almacenar los IDs de los libros afectados
    DECLARE @AffectedBooks TABLE (BookId INT);

    -- Insertar los BookId afectados por la operación insert/update/delete
    INSERT INTO @AffectedBooks
    SELECT DISTINCT BookId FROM inserted
    UNION
    SELECT DISTINCT BookId FROM deleted;

    -- Actualizar el promedio para cada libro afectado
    UPDATE Books
    SET Average = COALESCE((SELECT AVG(Stars) FROM Reviews WHERE Reviews.BookId = Books.Id), 0)
    FROM Books
    INNER JOIN @AffectedBooks ab ON Books.Id = ab.BookId;
END;
";

			migrationBuilder.Sql(sql);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_UpdateBookAverage;");
		}
	}
}
