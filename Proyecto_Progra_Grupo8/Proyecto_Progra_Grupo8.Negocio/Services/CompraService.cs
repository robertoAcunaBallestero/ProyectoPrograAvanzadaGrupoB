using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repository;

        public CompraService(ICompraRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            _repository = repository;
        }

        public Orden Comprar(
            string usuarioId,
            IDictionary<int, int> cantidades)
        {
            if (string.IsNullOrEmpty(usuarioId))
            {
                throw new InvalidOperationException(
                    "Debe iniciar sesión para realizar la compra.");
            }

            if (cantidades == null || cantidades.Count == 0)
            {
                throw new InvalidOperationException(
                    "El carrito está vacío.");
            }

            foreach (var item in cantidades)
            {
                if (item.Value <= 0)
                {
                    throw new InvalidOperationException(
                        "La cantidad de entradas debe ser mayor que cero.");
                }
            }

            return _repository.ProcesarCompra(
                usuarioId,
                cantidades);
        }

        public IEnumerable<Orden> Historial(
            string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
            {
                throw new InvalidOperationException(
                    "Debe iniciar sesión para consultar sus compras.");
            }

            return _repository.ObtenerOrdenesUsuario(
                usuarioId);
        }

        public Orden DetalleOrden(
            int ordenId,
            string usuarioId)
        {
            if (ordenId <= 0)
            {
                throw new InvalidOperationException(
                    "La orden indicada no es válida.");
            }

            if (string.IsNullOrEmpty(usuarioId))
            {
                throw new InvalidOperationException(
                    "Debe iniciar sesión para consultar la orden.");
            }

            return _repository.ObtenerOrdenUsuario(
                ordenId,
                usuarioId);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}