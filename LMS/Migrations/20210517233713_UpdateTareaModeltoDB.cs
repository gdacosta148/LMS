using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class UpdateTareaModeltoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreCurso",
                table: "CursoClases");

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_CursoId",
                table: "Tareas",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Curso_CursoId",
                table: "Tareas",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Curso_CursoId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_CursoId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Tareas");

            migrationBuilder.AddColumn<string>(
                name: "NombreCurso",
                table: "CursoClases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
