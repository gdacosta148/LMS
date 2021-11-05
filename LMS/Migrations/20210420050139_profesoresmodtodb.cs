using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class profesoresmodtodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TareaMateria_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMateria");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMateria_Tareas_TareaId",
                table: "TareaMateria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TareaMateria",
                table: "TareaMateria");

            migrationBuilder.RenameTable(
                name: "TareaMateria",
                newName: "TareaMaterias");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMateria_TareaId",
                table: "TareaMaterias",
                newName: "IX_TareaMaterias_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMateria_ProfesorMateriaId",
                table: "TareaMaterias",
                newName: "IX_TareaMaterias_ProfesorMateriaId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_TareaMaterias",
                table: "TareaMaterias",
                column: "Id");

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TareaMaterias_MateriaProfesors_ProfesorMateriaId",
                table: "TareaMaterias");

            migrationBuilder.DropForeignKey(
                name: "FK_TareaMaterias_Tareas_TareaId",
                table: "TareaMaterias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TareaMaterias",
                table: "TareaMaterias");

            migrationBuilder.DropColumn(
                name: "nombre_clase",
                table: "Profesores");

            migrationBuilder.DropColumn(
                name: "nombre_materia",
                table: "Profesores");

            migrationBuilder.RenameTable(
                name: "TareaMaterias",
                newName: "TareaMateria");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMaterias_TareaId",
                table: "TareaMateria",
                newName: "IX_TareaMateria_TareaId");

            migrationBuilder.RenameIndex(
                name: "IX_TareaMaterias_ProfesorMateriaId",
                table: "TareaMateria",
                newName: "IX_TareaMateria_ProfesorMateriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TareaMateria",
                table: "TareaMateria",
                column: "Id");

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
