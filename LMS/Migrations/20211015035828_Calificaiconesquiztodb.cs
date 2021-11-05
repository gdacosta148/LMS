using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class Calificaiconesquiztodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Calificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_QuizId",
                table: "Calificaciones",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calificaciones_Quiz_QuizId",
                table: "Calificaciones",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calificaciones_Quiz_QuizId",
                table: "Calificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Calificaciones_QuizId",
                table: "Calificaciones");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Calificaciones");
        }
    }
}
