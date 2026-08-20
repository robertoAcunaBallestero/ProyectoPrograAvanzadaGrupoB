using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Models.Api
{
    public class DashboardApiDto
    {
        public int TotalUsuarios { get; set; }

        public int UsuariosActivos { get; set; }

        public int UsuariosInactivos { get; set; }

        public int TotalEventosActivos { get; set; }

        public int TotalEventosProximos { get; set; }

        public int TotalOrdenes { get; set; }

        public int EntradasVendidas { get; set; }

        public decimal IngresosTotales { get; set; }

        public List<EventoBajoAforoDto> EventosBajoAforo { get; set; }

        public IDictionary<string, decimal> IngresosPorEvento { get; set; }
    }
}
