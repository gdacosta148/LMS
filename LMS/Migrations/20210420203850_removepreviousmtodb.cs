using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class removepreviousmtodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nombre_clase",
                table: "Profesores");

            migrationBuilder.DropColumn(
                name: "nombre_materia",
                table: "Profesores");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nombre_clase",
                table: "Profesores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_materia",
                table: "Profesores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
