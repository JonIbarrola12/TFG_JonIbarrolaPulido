using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHome.IL.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_EstadisticasPartidosEquipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadisticasEquiposPartidos",
                columns: table => new
                {
                    EstadisticaPartidoEquipoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PartidoId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    TirosAPuerta = table.Column<int>(type: "int", nullable: true),
                    TirosFuera = table.Column<int>(type: "int", nullable: true),
                    Posesion = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Faltas = table.Column<int>(type: "int", nullable: true),
                    Corners = table.Column<int>(type: "int", nullable: true),
                    TarjetasAmarillas = table.Column<int>(type: "int", nullable: true),
                    TarjetasRojas = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadisticasEquiposPartidos", x => x.EstadisticaPartidoEquipoId);
                    table.ForeignKey(
                        name: "FK_EstadisticasEquiposPartidos_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstadisticasEquiposPartidos_Partidos_PartidoId",
                        column: x => x.PartidoId,
                        principalTable: "Partidos",
                        principalColumn: "PartidoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasEquiposPartidos_EquipoId",
                table: "EstadisticasEquiposPartidos",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasEquiposPartidos_PartidoId_EquipoId",
                table: "EstadisticasEquiposPartidos",
                columns: new[] { "PartidoId", "EquipoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadisticasEquiposPartidos");
        }
    }
}
