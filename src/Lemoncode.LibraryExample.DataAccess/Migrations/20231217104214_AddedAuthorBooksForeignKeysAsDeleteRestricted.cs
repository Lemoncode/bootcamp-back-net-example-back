using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lemoncode.LibraryExample.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class AddedAuthorBooksForeignKeysAsDeleteRestricted : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AuthorBook_Authors_AuthorsId",
				table: "AuthorBook");

			migrationBuilder.DropForeignKey(
				name: "FK_AuthorBook_Books_BooksId",
				table: "AuthorBook");

			migrationBuilder.AddForeignKey(
				name: "FK_AuthorBook_Authors_AuthorsId",
				table: "AuthorBook",
				column: "AuthorsId",
				principalTable: "Authors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AuthorBook_Books_BooksId",
				table: "AuthorBook",
				column: "BooksId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AuthorBook_Authors_AuthorsId",
				table: "AuthorBook");

			migrationBuilder.DropForeignKey(
				name: "FK_AuthorBook_Books_BooksId",
				table: "AuthorBook");

			migrationBuilder.AddForeignKey(
				name: "FK_AuthorBook_Authors_AuthorsId",
				table: "AuthorBook",
				column: "AuthorsId",
				principalTable: "Authors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_AuthorBook_Books_BooksId",
				table: "AuthorBook",
				column: "BooksId",
				principalTable: "Books",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
