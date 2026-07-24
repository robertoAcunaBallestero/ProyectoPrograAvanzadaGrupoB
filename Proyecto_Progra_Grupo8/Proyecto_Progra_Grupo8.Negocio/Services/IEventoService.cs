using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{

    public interface IEventoService : IDisposable
    {

        IEnumerable<Evento> ObtenerTodos();


        IEnumerable<Evento> ObtenerCatalogo();


        Evento ObtenerDetalle(int eventoId);


        Evento ObtenerParaEdicion(int eventoId);


        int ContarActivos();


        bool CodigoDisponible(
            string codigoEvento,
            int eventoIdExcluido);

        ResultadoOperacion Crear(Evento evento);


        ResultadoOperacion Actualizar(Evento evento);


        ResultadoOperacion Desactivar(int eventoId);
    }
}
