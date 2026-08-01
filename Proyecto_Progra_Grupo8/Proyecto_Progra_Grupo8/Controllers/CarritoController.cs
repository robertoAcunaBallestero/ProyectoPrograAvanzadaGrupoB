using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Models;
using Proyecto_Progra_Grupo8.Negocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Progra_Grupo8.Controllers
{
    [Authorize(Roles = "Asociado")]
    public class CarritoController : Controller
    {
        private const string ClaveCarrito = "CarritoEntradas";
        private readonly ICarteleraService _cartelera = new CarteleraService();
        private readonly ICompraService _compras = new CompraService();

        private List<CarritoItemViewModel> ObtenerCarrito()
        {
            var carrito = Session[ClaveCarrito] as List<CarritoItemViewModel>;
            if (carrito == null) { carrito = new List<CarritoItemViewModel>(); Session[ClaveCarrito] = carrito; }
            return carrito;
        }

        public ActionResult Index() => View(ObtenerCarrito());

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Agregar(AgregarCarritoViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Detalle", "Cartelera", new { id = model.EventoId });
            var evento = _cartelera.ObtenerDetalle(model.EventoId);
            if (evento == null) return HttpNotFound();

            var carrito = ObtenerCarrito();
            var existente = carrito.FirstOrDefault(x => x.EventoId == evento.EventoId);
            int cantidadFinal = model.Cantidad + (existente?.Cantidad ?? 0);
            if (cantidadFinal > evento.EntradasDisponibles)
            {
                TempData["MensajeError"] = "La cantidad solicitada supera las entradas disponibles.";
                return RedirectToAction("Detalle", "Cartelera", new { id = model.EventoId });
            }

            if (existente == null)
                carrito.Add(new CarritoItemViewModel { EventoId = evento.EventoId, Nombre = evento.Nombre, Precio = evento.PrecioEntrada, Disponibles = evento.EntradasDisponibles, Cantidad = model.Cantidad });
            else existente.Cantidad = cantidadFinal;

            TempData["MensajeExito"] = "Entradas agregadas al carrito.";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Actualizar(int eventoId, int cantidad)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.EventoId == eventoId);
            if (item != null && cantidad >= 1 && cantidad <= item.Disponibles) item.Cantidad = cantidad;
            else TempData["MensajeError"] = "Cantidad inválida.";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Eliminar(int eventoId)
        {
            var carrito = ObtenerCarrito();
            carrito.RemoveAll(x => x.EventoId == eventoId);
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Comprar()
        {
            var carrito = ObtenerCarrito();
            try
            {
                var cantidades = carrito.ToDictionary(x => x.EventoId, x => x.Cantidad);
                var orden = _compras.Comprar(User.Identity.GetUserId(), cantidades);
                Session.Remove(ClaveCarrito);
                TempData["MensajeExito"] = "Compra realizada correctamente. Se generaron sus tickets.";
                return RedirectToAction("Detalle", "Ordenes", new { id = orden.OrdenId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _cartelera.Dispose(); _compras.Dispose(); }
            base.Dispose(disposing);
        }
    }
}