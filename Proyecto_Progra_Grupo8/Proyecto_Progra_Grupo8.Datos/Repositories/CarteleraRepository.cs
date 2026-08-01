using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public class CarteleraRepository : ICarteleraRepository
    {
        private readonly ComprasDbContext _context = new ComprasDbContext();

        public IEnumerable<Evento> ObtenerEventosActivos() => _context.Eventos.AsNoTracking()
            .Where(e => e.Activo).OrderBy(e => e.FechaHora).ToList();

        public Evento ObtenerEventoActivo(int eventoId) => _context.Eventos.AsNoTracking()
            .FirstOrDefault(e => e.EventoId == eventoId && e.Activo);

        public ImagenEvento ObtenerPrimeraImagen(int eventoId) => _context.ImagenesEventos.AsNoTracking()
            .Where(i => i.EventoId == eventoId).OrderBy(i => i.ImagenId).FirstOrDefault();

        public void Dispose() => _context.Dispose();
    }
}
