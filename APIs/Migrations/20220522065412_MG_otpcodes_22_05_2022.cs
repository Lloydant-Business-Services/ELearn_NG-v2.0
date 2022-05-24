using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class MG_otpcodes_22_05_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLive",
                table: "CLASS_MEETINGS",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OTP_CODE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfRetries = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    OTPStatus = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP_CODE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTP_CODE_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OTP_CODE_UserId",
                table: "OTP_CODE",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTP_CODE");

            migrationBuilder.DropColumn(
                name: "IsLive",
                table: "CLASS_MEETINGS");
        }
    }
}
