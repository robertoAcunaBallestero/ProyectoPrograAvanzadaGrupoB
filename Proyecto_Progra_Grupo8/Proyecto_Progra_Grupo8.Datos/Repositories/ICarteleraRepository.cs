using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public interface ICarteleraRepository : IDisposable
    {
        IEnumerable<Evento> ObtenerEventosActivos();

        Evento ObtenerEventoActivo(int eventoId);

        ImagenEvento ObtenerPrimeraImagen(int eventoId);
    }
}
