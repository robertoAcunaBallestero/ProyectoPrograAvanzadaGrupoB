using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public interface ICompraRepository : IDisposable
    {
        Orden ProcesarCompra(string usuarioId, IDictionary<int, int> cantidades);
        IEnumerable<Orden> ObtenerOrdenesUsuario(string usuarioId);
        Orden ObtenerOrdenUsuario(int ordenId, string usuarioId);
    }
}
