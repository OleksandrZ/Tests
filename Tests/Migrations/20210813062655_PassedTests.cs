using Microsoft.EntityFrameworkCore.Migrations;

namespace Tests.Migrations
{
    public partial class PassedTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_AspNetUsers_UserId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Tests_TestId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_TestId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "UserAnswers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAnswers",
                newName: "PassedTestsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_PassedTestsId");

            migrationBuilder.CreateTable(
                name: "PassedTests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    TestsUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassedTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassedTests_AspNetUsers_TestsUserId",
                        column: x => x.TestsUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassedTests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassedTests_TestId",
                table: "PassedTests",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_PassedTests_TestsUserId",
                table: "PassedTests",
                column: "TestsUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_PassedTests_PassedTestsId",
                table: "UserAnswers",
                column: "PassedTestsId",
                principalTable: "PassedTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_PassedTests_PassedTestsId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "PassedTests");

            migrationBuilder.RenameColumn(
                name: "PassedTestsId",
                table: "UserAnswers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_PassedTestsId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_UserId");

            migrationBuilder.AddColumn<string>(
                name: "TestId",
                table: "UserAnswers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_TestId",
                table: "UserAnswers",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_AspNetUsers_UserId",
                table: "UserAnswers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Tests_TestId",
                table: "UserAnswers",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
