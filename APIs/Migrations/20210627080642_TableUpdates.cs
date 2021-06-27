using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class TableUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedUpInstitutionId",
                table: "USER");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PERSON",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PERSON");

            migrationBuilder.AddColumn<long>(
                name: "SignedUpInstitutionId",
                table: "USER",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
