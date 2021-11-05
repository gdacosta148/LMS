using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class EnvioTareaToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Clase_ClaseId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_ClaseId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ClaseId",
                table: "Tareas");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Tareas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnvioTarea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalificacionId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvioTarea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvioTarea_Calificaciones_CalificacionId",
                        column: x => x.CalificacionId,
                        principalTable: "Calificaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvioTarea_CalificacionId",
                table: "EnvioTarea",
                column: "CalificacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvioTarea");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Tareas");

            migrationBuilder.AddColumn<int>(
                name: "ClaseId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ClaseId",
                table: "Tareas",
                column: "ClaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Clase_ClaseId",
                table: "Tareas",
                column: "ClaseId",
                principalTable: "Clase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
