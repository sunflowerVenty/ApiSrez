using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSrez.Migrations
{
    /// <inheritdoc />
    public partial class controllers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Order",
                newName: "TimeStart");

            migrationBuilder.RenameColumn(
                name: "id_Game",
                table: "Order",
                newName: "id_Order");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeFinish",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFinish",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "TimeStart",
                table: "Order",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "id_Order",
                table: "Order",
                newName: "id_Game");
        }
    }
}
