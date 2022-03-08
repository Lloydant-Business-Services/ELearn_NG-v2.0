using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class FacultyDeptSlug2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USER_SECURITY_QUESTION_SecurityQuestionId",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "IX_USER_SecurityQuestionId",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "SecurityQuestionId",
                table: "USER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SecurityQuestionId",
                table: "USER",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_SecurityQuestionId",
                table: "USER",
                column: "SecurityQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_USER_SECURITY_QUESTION_SecurityQuestionId",
                table: "USER",
                column: "SecurityQuestionId",
                principalTable: "SECURITY_QUESTION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
