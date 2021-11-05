using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class addTareastoCalificacionesFromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_TareaId",
                table: "Calificaciones",
                column: "TareaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calificaciones_Tareas_TareaId",
                table: "Calificaciones",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calificaciones_Tareas_TareaId",
                table: "Calificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_TareaId",
                table: "Calificaciones");
        }
    }
}
