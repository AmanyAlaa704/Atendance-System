using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "settings");

            migrationBuilder.RenameColumn(
                name: "LeaveTime",
                table: "settings",
                newName: "Time");

            migrationBuilder.AddColumn<string>(
                name: "FeildName",
                table: "settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeildName",
                table: "settings");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "settings",
                newName: "LeaveTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "settings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
