using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public interface IUsuarioService : IDisposable
    {
        IEnumerable<ApplicationUser> ObtenerTodos();

        ApplicationUser ObtenerPorId(string id);

        string ObtenerRol(string usuarioId);

        ResultadoOperacion Crear(
            ApplicationUser usuario,
            string contrasena,
            string rol);

        ResultadoOperacion Actualizar(
            ApplicationUser usuario,
            string rol);

        ResultadoOperacion Eliminar(string id);
    }
}
