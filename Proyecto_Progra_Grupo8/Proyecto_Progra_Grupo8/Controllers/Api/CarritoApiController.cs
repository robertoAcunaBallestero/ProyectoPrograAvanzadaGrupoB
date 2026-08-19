using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Proyecto_Progra_Grupo8.Filters;
using Proyecto_Progra_Grupo8.Infraestructura;
using Proyecto_Progra_Grupo8.Models;
using Proyecto_Progra_Grupo8.Models.Api;
using Proyecto_Progra_Grupo8.Negocio.Services;

namespace Proyecto_Progra_Grupo8.Controllers.Api
{
    [Authorize(Roles = "Asociado")]
    public class CarritoApiController : ApiController
    {
        private readonly ICarteleraService _cartelera;
        private readonly ICompraService _compras;

        public CarritoApiController(
            ICarteleraService cartelera,
            ICompraService compras)
        {
            _cartelera = cartelera;
            _compras = compras;
        }

        private HttpSessionStateBase Sesion =>
            new HttpSessionStateWrapper(HttpContext.Current.Session);

        // POST api/carrito/agregar
        [HttpPost]
        [ValidateModel]
        public IHttpActionResult Agregar(AgregarCarritoViewModel model)
        {
            var evento = _cartelera.ObtenerDetalle(model.EventoId);

            if (evento == null)
            {
                return NotFound();
            }

            var carrito = CarritoSesion.Obtener(Sesion);

            var existente = carrito.FirstOrDefault(
                x => x.EventoId == evento.EventoId);

            int cantidadFinal =
                model.Cantidad + (existente?.Cantidad ?? 0);

            if (cantidadFinal > evento.EntradasDisponibles)
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    new OperacionApiDto(
                        false,
                        "La cantidad solicitada supera las entradas disponibles."));
            }

            if (existente == null)
            {
                carrito.Add(new CarritoItemViewModel
                {
                    EventoId = evento.EventoId,
                    Nombre = evento.Nombre,
                    Precio = evento.PrecioEntrada,
                    Disponibles = evento.EntradasDisponibles,
                    Cantidad = model.Cantidad
                });
            }
            else
            {
                existente.Cantidad = cantidadFinal;
            }

            return Ok(new OperacionApiDto(
                true,
                "Entradas agregadas al carrito."));
        }

        // POST api/carrito/comprar
        [HttpPost]
        public IHttpActionResult Comprar()
        {
            var carrito = CarritoSesion.Obtener(Sesion);

            if (!carrito.Any())
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    new CompraResultDto
                    {
                        Exito = false,
                        Mensaje = "El carrito está vacío."
                    });
            }

            try
            {
                var cantidades = carrito.ToDictionary(
                    x => x.EventoId,
                    x => x.Cantidad);

                var orden = _compras.Comprar(
                    User.Identity.GetUserId(),
                    cantidades);

                CarritoSesion.Limpiar(Sesion);

                return Ok(new CompraResultDto
                {
                    Exito = true,
                    Mensaje = "Compra realizada correctamente. Se generaron sus tickets.",
                    OrdenId = orden.OrdenId
                });
            }
            catch (InvalidOperationException ex)
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    new CompraResultDto
                    {
                        Exito = false,
                        Mensaje = ex.Message
                    });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cartelera.Dispose();
                _compras.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
