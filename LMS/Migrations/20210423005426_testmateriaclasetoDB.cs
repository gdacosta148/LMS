using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class testmateriaclasetoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaseId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MateriaId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ClaseId",
                table: "Tareas",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_MateriaId",
                table: "Tareas",
                column: "MateriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Clase_ClaseId",
                table: "Tareas",
                column: "ClaseId",
                principalTable: "Clase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Materia_MateriaId",
                table: "Tareas",
                column: "MateriaId",
                principalTable: "Materia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Clase_ClaseId",
                table: "Tareas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Materia_MateriaId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_ClaseId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_MateriaId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ClaseId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "MateriaId",
                table: "Tareas");
        }
    }
}
