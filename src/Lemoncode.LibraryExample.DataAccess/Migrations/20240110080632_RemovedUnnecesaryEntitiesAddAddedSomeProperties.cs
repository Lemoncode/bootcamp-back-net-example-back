﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lemoncode.LibraryExample.DataAccess.Migrations
{
	/// <inheritdoc />
	public partial class RemovedUnnecesaryEntitiesAddAddedSomeProperties : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "BookDownloads");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.AddColumn<string>(
				name: "ImageFileName",
				table: "Books",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ImageFileName",
				table: "Books");

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "BookDownloads",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BookId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false),
					Date = table.Column<DateTime>(type: "datetime2", nullable: false),
					IPAddress = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false)
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
				name: "IX_BookDownloads_BookId",
				table: "BookDownloads",
				column: "BookId");

			migrationBuilder.CreateIndex(
				name: "IX_BookDownloads_UserId",
				table: "BookDownloads",
				column: "UserId");
		}
	}
}
