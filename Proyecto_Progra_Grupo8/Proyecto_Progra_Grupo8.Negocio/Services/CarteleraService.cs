using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public class CarteleraService : ICarteleraService
    {
        private readonly ICarteleraRepository _repository;
        public CarteleraService() : this(new CarteleraRepository()) { }
        public CarteleraService(ICarteleraRepository repository) { _repository = repository; }
        public IEnumerable<Evento> ObtenerCartelera() => _repository.ObtenerEventosActivos();
        public Evento ObtenerDetalle(int eventoId) => _repository.ObtenerEventoActivo(eventoId);
        public ImagenEvento ObtenerImagenPrincipal(int eventoId) => _repository.ObtenerPrimeraImagen(eventoId);
        public void Dispose() => _repository.Dispose();
    }
}
