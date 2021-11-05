using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class editcalificaiconestoDB12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Calificaciones",
                newName: "EndDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Calificaciones",
                newName: "Time");
        }
    }
}
