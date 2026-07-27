using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;
using System.Web; 

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public interface IEventoService : IDisposable
    {
        IEnumerable<Evento> ObtenerTodos();
        IEnumerable<Evento> ObtenerCatalogo();
        Evento ObtenerDetalle(int eventoId);
        Evento ObtenerParaEdicion(int eventoId);
        int ContarActivos();
        bool CodigoDisponible(string codigoEvento, int eventoIdExcluido);

        

        // Se añade la colección de imágenes que vienen desde el controlador
        ResultadoOperacion Crear(Evento evento, IEnumerable<HttpPostedFileBase> imagenes);

        // Se añade la colección para soportar la carga de nuevas imágenes en la edición
        ResultadoOperacion Actualizar(Evento evento, IEnumerable<HttpPostedFileBase> nuevasImagenes);

        

        ResultadoOperacion Desactivar(int eventoId);
    }
}