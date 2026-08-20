using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public interface ICarteleraService : IDisposable
    {
        IEnumerable<Evento> ObtenerCartelera();
        Evento ObtenerDetalle(int eventoId);
        ImagenEvento ObtenerImagenPrincipal(int eventoId);

        ImagenEvento ObtenerImagen(int imagenId);
    }
}
