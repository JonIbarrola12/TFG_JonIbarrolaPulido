using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHome.IL.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class Cambiar_campos_tablas_Ligas_Equipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Temporada",
                table: "Ligas",
                newName: "ExternalId");

            migrationBuilder.AlterColumn<string>(
                name: "NombreEstadio",
                table: "Equipos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "Equipos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ligas_ExternalId",
                table: "Ligas",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_ExternalId",
                table: "Equipos",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ligas_ExternalId",
                table: "Ligas");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_ExternalId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Equipos");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "Ligas",
                newName: "Temporada");

            migrationBuilder.UpdateData(
                table: "Equipos",
                keyColumn: "NombreEstadio",
                keyValue: null,
                column: "NombreEstadio",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NombreEstadio",
                table: "Equipos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
