namespace SportsHome.UI.Controllers.Resources
{
    public class PartidosResource
    {
        public int PartidoId { get; set; }
        public int ExternalId { get; set; }
        public int LigaId { get; set; }
        public int Temporada { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Ronda { get; set; }
        public string Arbitro { get; set; }
        public string ZonaHoraria { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
        public LigasResource Liga { get; set; }
        public EquiposResource EquipoLocal { get; set; }
        public EquiposResource EquipoVisitante { get; set; }
    }
}
