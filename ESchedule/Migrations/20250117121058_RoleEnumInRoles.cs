using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESchedule.Migrations
{
    /// <inheritdoc />
    public partial class RoleEnumInRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleEnum",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleEnum",
                table: "AspNetRoles");
        }
    }
}
