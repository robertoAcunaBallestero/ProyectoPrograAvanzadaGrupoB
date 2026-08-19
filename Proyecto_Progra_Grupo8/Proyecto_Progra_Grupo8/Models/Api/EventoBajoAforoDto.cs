using System;

namespace Proyecto_Progra_Grupo8.Models.Api
{
    public class EventoBajoAforoDto
    {
        public int EventoId { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaHora { get; set; }

        public int EntradasDisponibles { get; set; }

        public int AforoTotal { get; set; }
    }
}
