using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_App.Infrastructure.Persistence.Migrations
{
    public partial class TodoTagMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "TodoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TodoTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TagColour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_TagsId",
                table: "TodoItems",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoTags_TagsId",
                table: "TodoItems",
                column: "TagsId",
                principalTable: "TodoTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoTags_TagsId",
                table: "TodoItems");

            migrationBuilder.DropTable(
                name: "TodoTags");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_TagsId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "TodoItems");
        }
    }
}
