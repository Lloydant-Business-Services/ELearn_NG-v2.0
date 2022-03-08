using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class LiveLectures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLASS_MEETINGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Agenda = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Duration = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Start_Meeting_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Join_Meeting_Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLASS_MEETINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CLASS_MEETINGS_COURSE_CourseId",
                        column: x => x.CourseId,
                        principalTable: "COURSE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CLASS_MEETINGS_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLASS_MEETINGS_CourseId",
                table: "CLASS_MEETINGS",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CLASS_MEETINGS_UserId",
                table: "CLASS_MEETINGS",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLASS_MEETINGS");
        }
    }
}
