using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class CarteleraRepository : ICarteleraRepository
    {
        private readonly ProyectoDbContext _context;

        public CarteleraRepository(ProyectoDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        public IEnumerable<Evento> ObtenerEventosActivos()
        {
            return _context.Eventos
                .Include(e => e.CategoriaEvento)
                .Include(e => e.Imagenes)
                .AsNoTracking()
                .Where(e =>
                    e.Activo &&
                    e.FechaHora >= DateTime.Now)
                .OrderBy(e => e.FechaHora)
                .ToList();
        }

        public Evento ObtenerEventoActivo(int eventoId)
        {
            return _context.Eventos
                .Include(e => e.CategoriaEvento)
                .Include(e => e.Imagenes)
                .AsNoTracking()
                .FirstOrDefault(e =>
                    e.EventoId == eventoId &&
                    e.Activo);
        }

        public ImagenEvento ObtenerPrimeraImagen(int eventoId)
        {
            return _context.ImagenesEventos
                .AsNoTracking()
                .Where(i => i.EventoId == eventoId)
                .OrderBy(i => i.ImagenId)
                .FirstOrDefault();
        }

        public void Dispose()
        {

        }
    }
}