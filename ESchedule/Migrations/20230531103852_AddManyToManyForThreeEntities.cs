using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ESchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyForThreeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Classes_IdClass",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdClass",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdClass",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NameClass",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "NameTeacher",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodeToJoin",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdUserAdmin",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassViewModelLessonViewModel",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    LessonsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassViewModelLessonViewModel", x => new { x.ClassesId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_ClassViewModelLessonViewModel_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassViewModelLessonViewModel_Lessons_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassViewModelUserAccountViewModel",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    UsersAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassViewModelUserAccountViewModel", x => new { x.ClassesId, x.UsersAccountId });
                    table.ForeignKey(
                        name: "FK_ClassViewModelUserAccountViewModel_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassViewModelUserAccountViewModel_Users_UsersAccountId",
                        column: x => x.UsersAccountId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CodeToJoin", "Description", "IdUserAdmin", "Name" },
                values: new object[,]
                {
                    { 1, "TEST1", "1 описання класу", 3, "1 клас" },
                    { 2, "TEST2", "2 описання класу", 3, "2 клас" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassViewModelLessonViewModel_LessonsId",
                table: "ClassViewModelLessonViewModel",
                column: "LessonsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassViewModelUserAccountViewModel_UsersAccountId",
                table: "ClassViewModelUserAccountViewModel",
                column: "UsersAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassViewModelLessonViewModel");

            migrationBuilder.DropTable(
                name: "ClassViewModelUserAccountViewModel");

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CodeToJoin",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IdUserAdmin",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "IdClass",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NameClass",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameTeacher",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdClass",
                table: "Users",
                column: "IdClass");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Classes_IdClass",
                table: "Users",
                column: "IdClass",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
