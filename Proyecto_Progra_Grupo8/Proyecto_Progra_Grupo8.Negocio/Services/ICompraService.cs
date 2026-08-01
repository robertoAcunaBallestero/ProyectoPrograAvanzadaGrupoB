using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public interface ICompraService : IDisposable
    {
        Orden Comprar(string usuarioId, IDictionary<int, int> cantidades);
        IEnumerable<Orden> Historial(string usuarioId);
        Orden DetalleOrden(int ordenId, string usuarioId);
    }
}
