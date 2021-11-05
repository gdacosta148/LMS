using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class CalificacionesEdit14toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Calificado",
                table: "Calificaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoCalificado",
                table: "Calificaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calificado",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "NoCalificado",
                table: "Calificaciones");
        }
    }
}
