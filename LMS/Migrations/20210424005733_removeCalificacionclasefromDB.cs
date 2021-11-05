using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class removeCalificacionclasefromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalificacionClase");

            migrationBuilder.AddColumn<int>(
                name: "TareaId",
                table: "Calificaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TareaId",
                table: "Calificaciones");

            migrationBuilder.CreateTable(
                name: "CalificacionClase",
                columns: table => new
                {
                    CalificacionId = table.Column<int>(type: "int", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalificacionClase", x => new { x.CalificacionId, x.TareaId });
                    table.ForeignKey(
                        name: "FK_CalificacionClase_Calificaciones_CalificacionId",
                        column: x => x.CalificacionId,
                        principalTable: "Calificaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalificacionClase_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalificacionClase_TareaId",
                table: "CalificacionClase",
                column: "TareaId");
        }
    }
}
