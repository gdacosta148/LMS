using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class NoenviadoToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Noenviado",
                table: "Calificaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Noenviado",
                table: "Calificaciones");
        }
    }
}
