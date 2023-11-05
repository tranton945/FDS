using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDS.Migrations
{
    public partial class AddOldDocVer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Flight_FlightId",
                table: "Document");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Signature",
                table: "Document",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Document",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "OldDocVer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<float>(type: "real", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signature = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DocId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OldDocVer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OldDocVer_Document_DocId",
                        column: x => x.DocId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OldDocVer_DocId",
                table: "OldDocVer",
                column: "DocId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Flight_FlightId",
                table: "Document",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Flight_FlightId",
                table: "Document");

            migrationBuilder.DropTable(
                name: "OldDocVer");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Signature",
                table: "Document",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Flight_FlightId",
                table: "Document",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
