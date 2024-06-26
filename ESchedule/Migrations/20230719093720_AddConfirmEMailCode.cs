﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmEMailCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CodeToConfirmEmail",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeToConfirmEmail",
                table: "Users");
        }
    }
}
