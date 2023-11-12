using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDS.Migrations
{
    public partial class updateolddocver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "OldDocVer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "DocFile",
                table: "OldDocVer",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocFile",
                table: "OldDocVer");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "OldDocVer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Document",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
