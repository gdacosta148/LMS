using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class TemptoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "QuizOptions");

            migrationBuilder.CreateTable(
                name: "Temp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temp");

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "QuizOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
