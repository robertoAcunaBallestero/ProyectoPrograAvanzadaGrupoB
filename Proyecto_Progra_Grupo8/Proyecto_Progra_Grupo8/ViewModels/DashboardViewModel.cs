using Proyecto_Progra_Grupo8.Entidades.Models;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsuarios { get; set; }

        public int TotalEventosActivos { get; set; }

        public int TotalEventosProximos { get; set; }

        public int TotalOrdenes { get; set; }

        public int EntradasVendidas { get; set; }

        public decimal IngresosTotales { get; set; }

        public IEnumerable<Evento> EventosBajoAforo { get; set; }

        public IDictionary<string, decimal> IngresosPorEvento { get; set; }
    }
}