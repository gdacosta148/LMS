using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class removeclasetareastoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaseTareas_ClaseProfesors_ClaseProfesorId",
                table: "ClaseTareas");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaseTareas_Tareas_TareaId",
                table: "ClaseTareas");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMaterias_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMaterias");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMaterias_Tareas_TareaId",
                table: "TareaMaterias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TareaMaterias",
                table: "TareaMaterias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClaseTareas",
                table: "ClaseTareas");

            migrationBuilder.RenameTable(
                name: "TareaMaterias",
                newName: "TareaMateria");

            migrationBuilder.RenameTable(
                name: "ClaseTareas",
                newName: "ClaseTarea");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMaterias_TareaId",
                table: "TareaMateria",
                newName: "IX_TareaMateria_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMaterias_ProfesorMateriaId",
                table: "TareaMateria",
                newName: "IX_TareaMateria_ProfesorMateriaId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaseTareas_TareaId",
                table: "ClaseTarea",
                newName: "IX_ClaseTarea_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaseTareas_ClaseProfesorId",
                table: "ClaseTarea",
                newName: "IX_ClaseTarea_ClaseProfesorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TareaMateria",
                table: "TareaMateria",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClaseTarea",
                table: "ClaseTarea",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaseTarea_ClaseProfesors_ClaseProfesorId",
                table: "ClaseTarea",
                column: "ClaseProfesorId",
                principalTable: "ClaseProfesors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaseTarea_Tareas_TareaId",
                table: "ClaseTarea",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TareaMateria_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMateria",
                column: "ProfesorMateriaId",
                principalTable: "MateriaProfesors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TareaMateria_Tareas_TareaId",
                table: "TareaMateria",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaseTarea_ClaseProfesors_ClaseProfesorId",
                table: "ClaseTarea");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaseTarea_Tareas_TareaId",
                table: "ClaseTarea");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMateria_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMateria");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMateria_Tareas_TareaId",
                table: "TareaMateria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TareaMateria",
                table: "TareaMateria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClaseTarea",
                table: "ClaseTarea");

            migrationBuilder.RenameTable(
                name: "TareaMateria",
                newName: "TareaMaterias");

            migrationBuilder.RenameTable(
                name: "ClaseTarea",
                newName: "ClaseTareas");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMateria_TareaId",
                table: "TareaMaterias",
                newName: "IX_TareaMaterias_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMateria_ProfesorMateriaId",
                table: "TareaMaterias",
                newName: "IX_TareaMaterias_ProfesorMateriaId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaseTarea_TareaId",
                table: "ClaseTareas",
                newName: "IX_ClaseTareas_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaseTarea_ClaseProfesorId",
                table: "ClaseTareas",
                newName: "IX_ClaseTareas_ClaseProfesorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TareaMaterias",
                table: "TareaMaterias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClaseTareas",
                table: "ClaseTareas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaseTareas_ClaseProfesors_ClaseProfesorId",
                table: "ClaseTareas",
                column: "ClaseProfesorId",
                principalTable: "ClaseProfesors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaseTareas_Tareas_TareaId",
                table: "ClaseTareas",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TareaMaterias_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMaterias",
                column: "ProfesorMateriaId",
                principalTable: "MateriaProfesors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TareaMaterias_Tareas_TareaId",
                table: "TareaMaterias",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
