using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class Fix2toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalificacionTareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                  
                    EnvioTareaId = table.Column<int>(type: "int", nullable: true),
                    ComentarioProfesor = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalificacionTareas");
        }
    }
}
