using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly ComprasDbContext _context = new ComprasDbContext();

        public Orden ProcesarCompra(string usuarioId, IDictionary<int, int> cantidades)
        {
            using (var tx = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    var orden = new Orden { UsuarioId = usuarioId, FechaCompra = DateTime.Now };
                    decimal total = 0;

                    foreach (var item in cantidades)
                    {
                        var evento = _context.Eventos.FirstOrDefault(e => e.EventoId == item.Key && e.Activo);
                        if (evento == null) throw new InvalidOperationException("Uno de los eventos ya no está disponible.");
                        if (item.Value <= 0) throw new InvalidOperationException("La cantidad de entradas debe ser mayor que cero.");
                        if (evento.EntradasDisponibles < item.Value)
                            throw new InvalidOperationException("No hay suficientes entradas disponibles para " + evento.Nombre + ".");

                        evento.EntradasDisponibles -= item.Value;
                        var detalle = new DetalleOrden
                        {
                            EventoId = evento.EventoId,
                            Cantidad = item.Value,
                            PrecioUnitario = evento.PrecioEntrada
                        };

                        for (int i = 0; i < item.Value; i++)
                        {
                            detalle.Tickets.Add(new Ticket
                            {
                                CodigoUnico = Guid.NewGuid().ToString("N").ToUpperInvariant(),
                                FechaGeneracion = DateTime.Now
                            });
                        }

                        orden.Detalles.Add(detalle);
                        total += evento.PrecioEntrada * item.Value;
                    }

                    orden.Total = total;
                    _context.Ordenes.Add(orden);
                    _context.SaveChanges();
                    tx.Commit();
                    return orden;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<Orden> ObtenerOrdenesUsuario(string usuarioId) => _context.Ordenes.AsNoTracking()
            .Where(o => o.UsuarioId == usuarioId).OrderByDescending(o => o.FechaCompra).ToList();

        public Orden ObtenerOrdenUsuario(int ordenId, string usuarioId) => _context.Ordenes
            .Include(o => o.Detalles.Select(d => d.Evento))
            .Include(o => o.Detalles.Select(d => d.Tickets))
            .AsNoTracking().FirstOrDefault(o => o.OrdenId == ordenId && o.UsuarioId == usuarioId);

        public void Dispose() => _context.Dispose();
    }
}
