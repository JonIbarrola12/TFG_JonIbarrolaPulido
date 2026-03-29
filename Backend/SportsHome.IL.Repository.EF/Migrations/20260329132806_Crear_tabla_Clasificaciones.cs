using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsHome.IL.Repository.EF.Migrations
{
    /// <inheritdoc />
    public partial class Crear_tabla_Clasificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clasificaciones",
                columns: table => new
                {
                    ClasificacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LigaId = table.Column<int>(type: "int", nullable: false),
                    Temporada = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    Jugados = table.Column<int>(type: "int", nullable: false),
                    Ganados = table.Column<int>(type: "int", nullable: false),
                    Empatados = table.Column<int>(type: "int", nullable: false),
                    Perdidos = table.Column<int>(type: "int", nullable: false),
                    GolesAFavor = table.Column<int>(type: "int", nullable: false),
                    GolesEnContra = table.Column<int>(type: "int", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Posicion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clasificaciones", x => x.ClasificacionId);
                    table.ForeignKey(
                        name: "FK_Clasificaciones_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clasificaciones_Ligas_LigaId",
                        column: x => x.LigaId,
                        principalTable: "Ligas",
                        principalColumn: "LigaId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clasificaciones_EquipoId",
                table: "Clasificaciones",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clasificaciones_LigaId_EquipoId_Temporada",
                table: "Clasificaciones",
                columns: new[] { "LigaId", "EquipoId", "Temporada" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clasificaciones_LigaId_Temporada_Posicion",
                table: "Clasificaciones",
                columns: new[] { "LigaId", "Temporada", "Posicion" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clasificaciones");
        }
    }
}
