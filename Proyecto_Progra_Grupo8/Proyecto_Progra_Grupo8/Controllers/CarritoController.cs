using Proyecto_Progra_Grupo8.Infraestructura;
using Proyecto_Progra_Grupo8.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class CarritoController : Controller
    {
        private List<CarritoItemViewModel> ObtenerCarrito()
        {
            return CarritoSesion.Obtener(Session);
        }

        // Agregar entradas al carrito se hace ahora vía AJAX contra
        // POST api/carrito/agregar (CarritoApiController), sin recargar la página.
        public ActionResult Index()
        {
            return View(ObtenerCarrito());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(
            int eventoId,
            int cantidad)
        {
            var carrito = ObtenerCarrito();

            var item =
                carrito.FirstOrDefault(
                    x => x.EventoId == eventoId);

            if (item != null &&
                cantidad >= 1 &&
                cantidad <= item.Disponibles)
            {
                item.Cantidad = cantidad;
            }
            else
            {
                TempData["MensajeError"] =
                    "Cantidad inválida.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int eventoId)
        {
            var carrito = ObtenerCarrito();

            carrito.RemoveAll(
                x => x.EventoId == eventoId);

            return RedirectToAction("Index");
        }

        // La confirmación de compra se hace ahora vía AJAX contra
        // POST api/carrito/comprar (CarritoApiController), sin recargar la página.
    }
}
