using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class sortOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SESSION",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SEMESTER",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SESSION");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SEMESTER");
        }
    }
}
