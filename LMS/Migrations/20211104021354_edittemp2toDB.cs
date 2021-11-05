using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class edittemp2toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Temp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Temp",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
