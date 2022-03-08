using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class LiveLectureUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CLASS_MEETINGS_COURSE_CourseId",
                table: "CLASS_MEETINGS");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CLASS_MEETINGS",
                newName: "CourseAllocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CLASS_MEETINGS_CourseId",
                table: "CLASS_MEETINGS",
                newName: "IX_CLASS_MEETINGS_CourseAllocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CLASS_MEETINGS_COURSE_ALLOCATION_CourseAllocationId",
                table: "CLASS_MEETINGS",
                column: "CourseAllocationId",
                principalTable: "COURSE_ALLOCATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CLASS_MEETINGS_COURSE_ALLOCATION_CourseAllocationId",
                table: "CLASS_MEETINGS");

            migrationBuilder.RenameColumn(
                name: "CourseAllocationId",
                table: "CLASS_MEETINGS",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CLASS_MEETINGS_CourseAllocationId",
                table: "CLASS_MEETINGS",
                newName: "IX_CLASS_MEETINGS_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CLASS_MEETINGS_COURSE_CourseId",
                table: "CLASS_MEETINGS",
                column: "CourseId",
                principalTable: "COURSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
