using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class quizoptionedit2toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "QuizOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "QuizOptions");
        }
    }
}
