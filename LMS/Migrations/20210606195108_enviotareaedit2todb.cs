using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class enviotareaedit2todb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "EnvioTarea");

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "EnvioTarea",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "EnvioTarea");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "EnvioTarea",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
