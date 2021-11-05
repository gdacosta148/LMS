using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class cursoeditmodeltoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NombreCurso",
                table: "Curso",
                newName: "Nombre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Curso",
                newName: "NombreCurso");
        }
    }
}
