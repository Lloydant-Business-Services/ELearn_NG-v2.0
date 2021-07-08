using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class FacultyDeptSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityAnswer",
                table: "USER");

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "FACULTY_SCHOOL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "DEPARTMENT",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slug",
                table: "FACULTY_SCHOOL");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "DEPARTMENT");

            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswer",
                table: "USER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
