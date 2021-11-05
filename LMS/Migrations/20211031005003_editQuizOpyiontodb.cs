using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class editQuizOpyiontodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizQuestionId",
                table: "QuizOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizOptions_QuizQuestionId",
                table: "QuizOptions",
                column: "QuizQuestionId");

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
                name: "FK_QuizOptions_QuizQuestion_QuizQuestionId",
                table: "QuizOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuizOptions_QuizQuestionId",
                table: "QuizOptions");

            migrationBuilder.DropColumn(
                name: "QuizQuestionId",
                table: "QuizOptions");
        }
    }
}
