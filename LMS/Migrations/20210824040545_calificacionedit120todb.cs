using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class calificacionedit120todb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalificacionTareas");

            migrationBuilder.AddColumn<string>(
                name: "ComentarioProfesor",
                table: "Calificaciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "Calificaciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComentarioProfesor",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "Calificaciones");

            migrationBuilder.CreateTable(
                name: "CalificacionTareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComentarioProfesor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvioTareaId = table.Column<int>(type: "int", nullable: true),
                    EnviotareaId = table.Column<int>(type: "int", nullable: false),
                    StatusMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalificacionTareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalificacionTareas_EnvioTarea_EnvioTareaId",
                        column: x => x.EnvioTareaId,
                        principalTable: "EnvioTarea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalificacionTareas_EnvioTareaId",
                table: "CalificacionTareas",
                column: "EnvioTareaId");
        }
    }
}
