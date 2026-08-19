using Proyecto_Progra_Grupo8.Entidades.Models;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Datos.Repositories
{
    public interface IResenaRepository : IDisposable
    {
        IEnumerable<Resena> ObtenerPendientes();

        IEnumerable<Resena> ObtenerAprobadasPorEvento(
            int eventoId);

        IEnumerable<Resena> ObtenerPorUsuario(
            string usuarioId);

        Resena ObtenerPorId(int resenaId);

        bool UsuarioComproEvento(
            int eventoId,
            string usuarioId);

        bool ExisteResena(
            int eventoId,
            string usuarioId);

        void Agregar(Resena resena);

        void Actualizar(Resena resena);

        void Guardar();
    }
}
