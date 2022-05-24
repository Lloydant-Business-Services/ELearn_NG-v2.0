using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class MG_numofretries_22_05_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRetries",
                table: "OTP_CODE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRetries",
                table: "OTP_CODE",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
