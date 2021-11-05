using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class EnvioTareeDIT1toDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComentarioProfesor",
                table: "EnvioTarea",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "EnvioTarea",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "EnvioTarea",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComentarioProfesor",
                table: "EnvioTarea");

            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "EnvioTarea");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "EnvioTarea");
        }
    }
}
