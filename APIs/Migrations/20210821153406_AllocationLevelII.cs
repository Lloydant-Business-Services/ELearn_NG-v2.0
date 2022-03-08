using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class AllocationLevelII : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LevelId",
                table: "COURSE_ALLOCATION",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LevelId",
                table: "COURSE",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_LevelId",
                table: "COURSE_ALLOCATION",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_COURSE_ALLOCATION_LEVEL_LevelId",
                table: "COURSE_ALLOCATION",
                column: "LevelId",
                principalTable: "LEVEL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COURSE_ALLOCATION_LEVEL_LevelId",
                table: "COURSE_ALLOCATION");

            migrationBuilder.DropIndex(
                name: "IX_COURSE_ALLOCATION_LevelId",
                table: "COURSE_ALLOCATION");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "COURSE_ALLOCATION");

            migrationBuilder.AlterColumn<long>(
                name: "LevelId",
                table: "COURSE",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
