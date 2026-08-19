using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class ResenaRepository : IResenaRepository
    {
        private readonly ProyectoDbContext _context;

        public ResenaRepository(ProyectoDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public IEnumerable<Resena> ObtenerPendientes()
        {
            return _context.Resenas
                .Include(r => r.Evento)
                .Include(r => r.Usuario)
                .AsNoTracking()
                .Where(r => r.Estado == "Pendiente")
                .OrderBy(r => r.FechaResena)
                .ToList();
        }

        public IEnumerable<Resena> ObtenerAprobadasPorEvento(
            int eventoId)
        {
            return _context.Resenas
                .Include(r => r.Usuario)
                .AsNoTracking()
                .Where(r =>
                    r.EventoId == eventoId &&
                    r.Estado == "Aprobada")
                .OrderByDescending(r => r.FechaResena)
                .ToList();
        }

        public IEnumerable<Resena> ObtenerPorUsuario(
            string usuarioId)
        {
            return _context.Resenas
                .Include(r => r.Evento)
                .AsNoTracking()
                .Where(r => r.UsuarioId == usuarioId)
                .OrderByDescending(r => r.FechaResena)
                .ToList();
        }

        public Resena ObtenerPorId(int resenaId)
        {
            return _context.Resenas
                .Include(r => r.Evento)
                .Include(r => r.Usuario)
                .FirstOrDefault(r =>
                    r.ResenaId == resenaId);
        }

        public bool UsuarioComproEvento(
            int eventoId,
            string usuarioId)
        {
            return _context.DetallesOrden
                .Any(d =>
                    d.EventoId == eventoId &&
                    d.Orden.UsuarioId == usuarioId);
        }

        public bool ExisteResena(
            int eventoId,
            string usuarioId)
        {
            return _context.Resenas
                .Any(r =>
                    r.EventoId == eventoId &&
                    r.UsuarioId == usuarioId);
        }

        public void Agregar(Resena resena)
        {
            if (resena == null)
            {
                throw new ArgumentNullException(nameof(resena));
            }

            _context.Resenas.Add(resena);
        }

        public void Actualizar(Resena resena)
        {
            if (resena == null)
            {
                throw new ArgumentNullException(nameof(resena));
            }

            _context.Entry(resena).State =
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
