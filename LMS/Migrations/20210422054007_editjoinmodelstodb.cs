using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class editjoinmodelstodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreMateria",
                table: "TareaMaterias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreClase",
                table: "ClaseTareas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreMateria",
                table: "TareaMaterias");

            migrationBuilder.DropColumn(
                name: "NombreClase",
                table: "ClaseTareas");
        }
    }
}
