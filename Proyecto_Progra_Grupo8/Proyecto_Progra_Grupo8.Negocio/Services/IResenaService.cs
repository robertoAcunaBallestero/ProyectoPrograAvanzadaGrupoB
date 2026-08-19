using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public interface IResenaService : IDisposable
    {
        IEnumerable<Resena> ObtenerPendientes();

        IEnumerable<Resena> ObtenerAprobadasPorEvento(
            int eventoId);

        IEnumerable<Resena> ObtenerPorUsuario(
            string usuarioId);

        Resena ObtenerPorId(int resenaId);

        ResultadoOperacion Crear(
            int eventoId,
            string usuarioId,
            string comentario);

        ResultadoOperacion Aprobar(int resenaId);

        ResultadoOperacion Rechazar(int resenaId);
    }
}
