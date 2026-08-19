using System.Collections.Generic;
using System.Web;
using Proyecto_Progra_Grupo8.Models;

namespace Proyecto_Progra_Grupo8.Infraestructura
{
    public static class CarritoSesion
    {
        public const string Clave = "CarritoEntradas";

        public static List<CarritoItemViewModel> Obtener(
            HttpSessionStateBase session)
        {
            var carrito =
                session[Clave] as List<CarritoItemViewModel>;

            if (carrito == null)
            {
                carrito = new List<CarritoItemViewModel>();
                session[Clave] = carrito;
            }

            return carrito;
        }

        public static void Limpiar(HttpSessionStateBase session)
        {
            session.Remove(Clave);
        }
    }
}
