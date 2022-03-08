using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "PERSON",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "PERSON",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "PERSON",
                newName: "Othername");

            migrationBuilder.AlterColumn<long>(
                name: "GenderId",
                table: "PERSON",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "PERSON",
                newName: "SurName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "PERSON",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Othername",
                table: "PERSON",
                newName: "LastName");

            migrationBuilder.AlterColumn<long>(
                name: "GenderId",
                table: "PERSON",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
