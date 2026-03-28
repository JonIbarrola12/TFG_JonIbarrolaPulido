namespace SportsHome.Core.Entities
{
    public class Jugadores
    {
        public int JugadorId { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string NombrePropio { get; set; }
        public string Apellido { get; set; }
        public int? Edad { get; set; }
        public string Nacionalidad { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }
        public string Foto { get; set; }

        public ICollection<JugadoresEquipos> JugadoresEquipos { get; set; }
    }
}
