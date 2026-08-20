using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly ProyectoDbContext _context;

        public CompraRepository(ProyectoDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        public Orden ProcesarCompra(
            string usuarioId,
            IDictionary<int, int> cantidades)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                throw new ArgumentException(
                    "El identificador del usuario es obligatorio.",
                    "usuarioId");
            }

            if (cantidades == null || !cantidades.Any())
            {
                throw new InvalidOperationException(
                    "El carrito no contiene entradas.");
            }

            using (var transaccion =
                _context.Database.BeginTransaction(
                    IsolationLevel.Serializable))
            {
                try
                {
                    var orden = new Orden
                    {
                        UsuarioId = usuarioId,
                        FechaCompra = DateTime.Now
                    };

                    decimal total = 0m;

                    // Se ordenan los eventos para reducir el riesgo
                    // de bloqueos cruzados entre compras simultáneas.
                    foreach (var item in cantidades.OrderBy(x => x.Key))
                    {
                        int eventoId = item.Key;
                        int cantidad = item.Value;

                        if (cantidad <= 0)
                        {
                            throw new InvalidOperationException(
                                "La cantidad de entradas debe ser mayor que cero.");
                        }

                        var evento = _context.Eventos
                            .SingleOrDefault(e =>
                                e.EventoId == eventoId &&
                                e.Activo);

                        if (evento == null)
                        {
                            throw new InvalidOperationException(
                                "Uno de los eventos ya no está disponible.");
                        }

                        if (evento.FechaHora <= DateTime.Now)
                        {
                            throw new InvalidOperationException(
                                "El evento " + evento.Nombre +
                                " ya inició o finalizó.");
                        }

                        if (evento.EntradasDisponibles < cantidad)
                        {
                            throw new InvalidOperationException(
                                "No hay suficientes entradas disponibles para " +
                                evento.Nombre + ".");
                        }

                        // Reduce el aforo dentro de la misma transacción.
                        evento.EntradasDisponibles -= cantidad;

                        var detalle = new DetalleOrden
                        {
                            EventoId = evento.EventoId,
                            Cantidad = cantidad,
                            PrecioUnitario = evento.PrecioEntrada
                        };

                        // Genera un ticket individual por cada entrada.
                        for (int i = 0; i < cantidad; i++)
                        {
                            detalle.Tickets.Add(
                                new Ticket
                                {
                                    CodigoUnico = Guid.NewGuid()
                                        .ToString("N")
                                        .ToUpperInvariant(),

                                    FechaGeneracion = DateTime.Now
                                });
                        }

                        orden.Detalles.Add(detalle);

                        total += evento.PrecioEntrada * cantidad;
                    }

                    orden.Total = total;

                    _context.Ordenes.Add(orden);
                    _context.SaveChanges();

                    transaccion.Commit();

                    return orden;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        //public IEnumerable<Orden> ObtenerOrdenesUsuario(
        //    string usuarioId)
        //{
        //    return _context.Ordenes
        //        .AsNoTracking()
        //        .Where(o => o.UsuarioId == usuarioId)
        //        .OrderByDescending(o => o.FechaCompra)
        //        .ToList();
        //} 

        public IEnumerable<Orden> ObtenerOrdenesUsuario(
            string usuarioId)
        {
            return _context.Ordenes
                .Include(o => o.Detalles.Select(d => d.Evento))
                .Include(o => o.Detalles.Select(d => d.Tickets))
                .AsNoTracking()
                .Where(o => o.UsuarioId == usuarioId)
                .OrderByDescending(o => o.FechaCompra)
                .ToList();
        }

        public Orden ObtenerOrdenUsuario(
            int ordenId,
            string usuarioId)
        {
            return _context.Ordenes
                .Include(o => o.Detalles.Select(d => d.Evento))
                .Include(o => o.Detalles.Select(d => d.Tickets))
                .AsNoTracking()
                .FirstOrDefault(o =>
                    o.OrdenId == ordenId &&
                    o.UsuarioId == usuarioId);
        }

        public int ContarOrdenes()
        {
            return _context.Ordenes.Count();
        }

        public int ContarEntradasVendidas()
        {
            return _context.DetallesOrden
                .Sum(d => (int?)d.Cantidad) ?? 0;
        }

        public decimal ObtenerIngresosTotales()
        {
            return _context.Ordenes
                .Sum(o => (decimal?)o.Total) ?? 0m;
        }

        public IDictionary<string, decimal>
            ObtenerIngresosPorEvento()
        {
            return _context.DetallesOrden
                .GroupBy(d => d.Evento.Nombre)
                .Select(g => new
                {
                    Evento = g.Key,
                    Total = g.Sum(d =>
                        d.PrecioUnitario * d.Cantidad)
                })
                .ToList()
                .ToDictionary(
                    x => x.Evento,
                    x => x.Total);
        }

        public void Dispose()
        {

        }
    }
}