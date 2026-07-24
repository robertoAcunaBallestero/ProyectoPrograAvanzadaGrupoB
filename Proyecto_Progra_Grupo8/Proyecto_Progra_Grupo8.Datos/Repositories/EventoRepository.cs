using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{

    public class EventoRepository : IEventoRepository
    {
        private readonly ProyectoDbContext _context;

        public EventoRepository(ProyectoDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }


        public IEnumerable<Evento> ObtenerTodos()
        {
            return _context.Eventos
                .AsNoTracking()
                .OrderBy(e => e.FechaHora)
                .ThenBy(e => e.Nombre)
                .ToList();
        }


        public IEnumerable<Evento> ObtenerActivos()
        {
            return _context.Eventos
                .AsNoTracking()
                .Where(e => e.Activo)
                .OrderBy(e => e.FechaHora)
                .ThenBy(e => e.Nombre)
                .ToList();
        }


        public Evento ObtenerPorId(int eventoId)
        {
            return _context.Eventos.Find(eventoId);
        }


        public bool ExisteCodigo(
            string codigoEvento,
            int eventoIdExcluido)
        {
            return _context.Eventos.Any(e =>
                e.CodigoEvento == codigoEvento &&
                e.EventoId != eventoIdExcluido);
        }


        public int ContarActivos()
        {
            return _context.Eventos.Count(e => e.Activo);
        }


        public void Agregar(Evento evento)
        {
            if (evento == null)
            {
                throw new ArgumentNullException(nameof(evento));
            }

            _context.Eventos.Add(evento);
        }


        public void Actualizar(Evento evento)
        {
            if (evento == null)
            {
                throw new ArgumentNullException(nameof(evento));
            }

            _context.Entry(evento).State =
                EntityState.Modified;
        }


        public void Guardar()
        {
            _context.SaveChanges();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
