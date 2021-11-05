using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class removeClasetareaandTareaMateriasfromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaseTarea");

            migrationBuilder.DropTable(
                name: "TareaMateria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaseTarea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaseProfesorId = table.Column<int>(type: "int", nullable: false),
                    NombreClase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TareaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseTarea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaseTarea_ClaseProfesors_ClaseProfesorId",
                        column: x => x.ClaseProfesorId,
                        principalTable: "ClaseProfesors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaseTarea_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareaMateria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMateria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfesorMateriaId = table.Column<int>(type: "int", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaMateria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TareaMateria_MateriaProfesors_ProfesorMateriaId",
                        column: x => x.ProfesorMateriaId,
                        principalTable: "MateriaProfesors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareaMateria_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaseTarea_ClaseProfesorId",
                table: "ClaseTarea",
                column: "ClaseProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseTarea_TareaId",
                table: "ClaseTarea",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaMateria_ProfesorMateriaId",
                table: "TareaMateria",
                column: "ProfesorMateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TareaMateria_TareaId",
                table: "TareaMateria",
                column: "TareaId");
        }
    }
}
