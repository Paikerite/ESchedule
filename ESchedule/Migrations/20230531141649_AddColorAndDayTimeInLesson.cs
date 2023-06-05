using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ESchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddColorAndDayTimeInLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "ColorCard",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DayTime",
                table: "Lessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCard",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "DayTime",
                table: "Lessons");

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CodeToJoin", "Description", "IdUserAdmin", "Name" },
                values: new object[,]
                {
                    { 1, "TEST1", "1 описання класу", 3, "1 клас" },
                    { 2, "TEST2", "2 описання класу", 3, "2 клас" }
                });
        }
    }
}
