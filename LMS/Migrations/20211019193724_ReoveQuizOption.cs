using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class ReoveQuizOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAnswers_QuizOption_QuizOptionId",
                table: "QuizAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizOption_QuizQuestion_QuizQuestionId",
                table: "QuizOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizOption",
                table: "QuizOption");

            migrationBuilder.RenameTable(
                name: "QuizOption",
                newName: "QuizOptions");

            migrationBuilder.RenameIndex(
                name: "IX_QuizOption_QuizQuestionId",
                table: "QuizOptions",
                newName: "IX_QuizOptions_QuizQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizOptions",
                table: "QuizOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAnswers_QuizOptions_QuizOptionId",
                table: "QuizAnswers",
                column: "QuizOptionId",
                principalTable: "QuizOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizOptions_QuizQuestion_QuizQuestionId",
                table: "QuizOptions",
                column: "QuizQuestionId",
                principalTable: "QuizQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAnswers_QuizOptions_QuizOptionId",
                table: "QuizAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizOptions_QuizQuestion_QuizQuestionId",
                table: "QuizOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizOptions",
                table: "QuizOptions");

            migrationBuilder.RenameTable(
                name: "QuizOptions",
                newName: "QuizOption");

            migrationBuilder.RenameIndex(
                name: "IX_QuizOptions_QuizQuestionId",
                table: "QuizOption",
                newName: "IX_QuizOption_QuizQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizOption",
                table: "QuizOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAnswers_QuizOption_QuizOptionId",
                table: "QuizAnswers",
                column: "QuizOptionId",
                principalTable: "QuizOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizOption_QuizQuestion_QuizQuestionId",
                table: "QuizOption",
                column: "QuizQuestionId",
                principalTable: "QuizQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
