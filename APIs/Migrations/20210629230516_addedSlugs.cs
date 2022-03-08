using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class addedSlugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MatricNoSlug",
                table: "STUDENT_PERSON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseCodeSlug",
                table: "COURSE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseTitleSlug",
                table: "COURSE",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatricNoSlug",
                table: "STUDENT_PERSON");

            migrationBuilder.DropColumn(
                name: "CourseCodeSlug",
                table: "COURSE");

            migrationBuilder.DropColumn(
                name: "CourseTitleSlug",
                table: "COURSE");
        }
    }
}
