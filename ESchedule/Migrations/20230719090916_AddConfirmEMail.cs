using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmEMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmEmail",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmEmail",
                table: "Users");
        }
    }
}
