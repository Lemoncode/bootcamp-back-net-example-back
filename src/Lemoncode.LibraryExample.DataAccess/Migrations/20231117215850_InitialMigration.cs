using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lemoncode.LibraryExample.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class InitialMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Authors",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Authors", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Books",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
					ImageAltText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Books", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AuthorBook",
				columns: table => new
				{
					AuthorsId = table.Column<int>(type: "int", nullable: false),
					BooksId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
					table.ForeignKey(
						name: "FK_AuthorBook_Authors_AuthorsId",
						column: x => x.AuthorsId,
						principalTable: "Authors",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AuthorBook_Books_BooksId",
						column: x => x.BooksId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Reviews",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BookId = table.Column<int>(type: "int", nullable: false),
					Reviewer = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reviews", x => x.Id);
					table.ForeignKey(
						name: "FK_Reviews_Books_BookId",
						column: x => x.BookId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "BookDownloads",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BookId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false),
					IPAddress = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false),
					Date = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BookDownloads", x => x.Id);
					table.ForeignKey(
						name: "FK_BookDownloads_Books_BookId",
						column: x => x.BookId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_BookDownloads_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AuthorBook_BooksId",
				table: "AuthorBook",
				column: "BooksId");

			migrationBuilder.CreateIndex(
				name: "IX_BookDownloads_BookId",
				table: "BookDownloads",
				column: "BookId");

			migrationBuilder.CreateIndex(
				name: "IX_BookDownloads_UserId",
				table: "BookDownloads",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Reviews_BookId",
				table: "Reviews",
				column: "BookId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AuthorBook");

			migrationBuilder.DropTable(
				name: "BookDownloads");

			migrationBuilder.DropTable(
				name: "Reviews");

			migrationBuilder.DropTable(
				name: "Authors");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "Books");
		}
	}
}
