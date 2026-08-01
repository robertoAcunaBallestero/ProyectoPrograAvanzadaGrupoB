using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repository;
        public CompraService() : this(new CompraRepository()) { }
        public CompraService(ICompraRepository repository) { _repository = repository; }

        public Orden Comprar(string usuarioId, IDictionary<int, int> cantidades)
        {
            if (string.IsNullOrWhiteSpace(usuarioId)) throw new InvalidOperationException("Debe iniciar sesión para comprar.");
            if (cantidades == null || cantidades.Count == 0) throw new InvalidOperationException("El carrito está vacío.");
            return _repository.ProcesarCompra(usuarioId, cantidades);
        }
        public IEnumerable<Orden> Historial(string usuarioId) => _repository.ObtenerOrdenesUsuario(usuarioId);
        public Orden DetalleOrden(int ordenId, string usuarioId) => _repository.ObtenerOrdenUsuario(ordenId, usuarioId);
        public void Dispose() => _repository.Dispose();
    }
}
