namespace Peschu.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class removedPublishedAndEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edited",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Articles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Edited",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "Articles",
                nullable: true);
        }
    }
}
