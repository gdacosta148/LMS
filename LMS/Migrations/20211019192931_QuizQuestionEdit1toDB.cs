using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class QuizQuestionEdit1toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuizOption1",
                table: "QuizQuestion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizOption2",
                table: "QuizQuestion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizOption3",
                table: "QuizQuestion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizOption4",
                table: "QuizQuestion",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizOption1",
                table: "QuizQuestion");

            migrationBuilder.DropColumn(
                name: "QuizOption2",
                table: "QuizQuestion");

            migrationBuilder.DropColumn(
                name: "QuizOption3",
                table: "QuizQuestion");

            migrationBuilder.DropColumn(
                name: "QuizOption4",
                table: "QuizQuestion");
        }
    }
}
