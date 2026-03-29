using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHome.IL.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_EstadisticasJugadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadisticasJugadores",
                columns: table => new
                {
                    EstadisticaJugadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    LigaId = table.Column<int>(type: "int", nullable: false),
                    Temporada = table.Column<int>(type: "int", nullable: false),
                    Apariciones = table.Column<int>(type: "int", nullable: true),
                    Goles = table.Column<int>(type: "int", nullable: true),
                    Asistencias = table.Column<int>(type: "int", nullable: true),
                    TarjetasAmarillas = table.Column<int>(type: "int", nullable: true),
                    TarjetasRojas = table.Column<int>(type: "int", nullable: true),
                    Minutos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadisticasJugadores", x => x.EstadisticaJugadorId);
                    table.ForeignKey(
                        name: "FK_EstadisticasJugadores_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstadisticasJugadores_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstadisticasJugadores_Ligas_LigaId",
                        column: x => x.LigaId,
                        principalTable: "Ligas",
                        principalColumn: "LigaId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasJugadores_EquipoId",
                table: "EstadisticasJugadores",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasJugadores_JugadorId_EquipoId_LigaId_Temporada",
                table: "EstadisticasJugadores",
                columns: new[] { "JugadorId", "EquipoId", "LigaId", "Temporada" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasJugadores_LigaId",
                table: "EstadisticasJugadores",
                column: "LigaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadisticasJugadores");
        }
    }
}
