using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class DeptHeadTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEPARTMENT_HEADS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT_HEADS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_HEADS_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_HEADS_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_HEADS_DepartmentId",
                table: "DEPARTMENT_HEADS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_HEADS_UserId",
                table: "DEPARTMENT_HEADS",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEPARTMENT_HEADS");
        }
    }
}
