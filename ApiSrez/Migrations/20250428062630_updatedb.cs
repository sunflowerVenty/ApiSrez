using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSrez.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Games",
                newName: "NameGame");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameGame",
                table: "Games",
                newName: "Name");
        }
    }
}
