using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    
    public interface IEventoRepository : IDisposable
    {

        IEnumerable<Evento> ObtenerTodos();


        IEnumerable<Evento> ObtenerActivos();


        Evento ObtenerPorId(int eventoId);


        bool ExisteCodigo(
            string codigoEvento,
            int eventoIdExcluido);


        int ContarActivos();


        void Agregar(Evento evento);


        void Actualizar(Evento evento);


        void Guardar();
    }
}
