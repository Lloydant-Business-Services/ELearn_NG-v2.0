using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class InstructorDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "FACULTY_SCHOOL",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "DEPARTMENT",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "INSTRUCTOR_DEPARTMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INSTRUCTOR_DEPARTMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_CourseAllocationId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_DepartmentId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_UserId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INSTRUCTOR_DEPARTMENT");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "FACULTY_SCHOOL");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "DEPARTMENT");
        }
    }
}
