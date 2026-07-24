using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{

    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;


        public EventoService(IEventoRepository eventoRepository)
        {
            if (eventoRepository == null)
            {
                throw new ArgumentNullException(
                    nameof(eventoRepository));
            }

            _eventoRepository = eventoRepository;
        }


        public IEnumerable<Evento> ObtenerTodos()
        {
            return _eventoRepository.ObtenerTodos();
        }


        public IEnumerable<Evento> ObtenerCatalogo()
        {
            return _eventoRepository.ObtenerActivos();
        }


        public Evento ObtenerDetalle(int eventoId)
        {
            if (eventoId <= 0)
            {
                return null;
            }

            return _eventoRepository.ObtenerPorId(eventoId);
        }


        public Evento ObtenerParaEdicion(int eventoId)
        {
            if (eventoId <= 0)
            {
                return null;
            }

            return _eventoRepository.ObtenerPorId(eventoId);
        }


        public int ContarActivos()
        {
            return _eventoRepository.ContarActivos();
        }


        public bool CodigoDisponible(
            string codigoEvento,
            int eventoIdExcluido)
        {
            if (string.IsNullOrWhiteSpace(codigoEvento))
            {
                return false;
            }

            string codigoNormalizado =
                codigoEvento.Trim().ToUpperInvariant();

            bool existe =
                _eventoRepository.ExisteCodigo(
                    codigoNormalizado,
                    eventoIdExcluido);

            return !existe;
        }


        public ResultadoOperacion Crear(Evento evento)
        {
            ResultadoOperacion validacion =
                ValidarDatosGenerales(evento);

            if (!validacion.Exito)
            {
                return validacion;
            }

            if (evento.FechaHora <= DateTime.Now)
            {
                return new ResultadoOperacion(
                    false,
                    "La fecha y hora del evento deben ser futuras.");
            }

            NormalizarTextos(evento);

            bool codigoRepetido =
                _eventoRepository.ExisteCodigo(
                    evento.CodigoEvento,
                    0);

            if (codigoRepetido)
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe un evento con el mismo código.");
            }


            evento.EntradasDisponibles =
                evento.AforoTotal;

            evento.Activo = true;

            try
            {
                _eventoRepository.Agregar(evento);
                _eventoRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "El evento fue creado correctamente.",
                    evento);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al guardar el evento.");
            }
        }


        public ResultadoOperacion Actualizar(Evento evento)
        {
            ResultadoOperacion validacion =
                ValidarDatosGenerales(evento);

            if (!validacion.Exito)
            {
                return validacion;
            }

            if (evento.EventoId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "El identificador del evento no es válido.");
            }

            Evento eventoExistente =
                _eventoRepository.ObtenerPorId(
                    evento.EventoId);

            if (eventoExistente == null)
            {
                return new ResultadoOperacion(
                    false,
                    "El evento indicado no existe.");
            }

            NormalizarTextos(evento);

            bool codigoRepetido =
                _eventoRepository.ExisteCodigo(
                    evento.CodigoEvento,
                    evento.EventoId);

            if (codigoRepetido)
            {
                return new ResultadoOperacion(
                    false,
                    "Ya existe otro evento con el mismo código.");
            }


            eventoExistente.CodigoEvento =
                evento.CodigoEvento;

            eventoExistente.Nombre =
                evento.Nombre;

            eventoExistente.Descripcion =
                evento.Descripcion;

            eventoExistente.CategoriaEventoId =
                evento.CategoriaEventoId;

            eventoExistente.FechaHora =
                evento.FechaHora;

            eventoExistente.Lugar =
                evento.Lugar;

            eventoExistente.PrecioEntrada =
                evento.PrecioEntrada;


            int entradasVendidas =
                eventoExistente.AforoTotal -
                eventoExistente.EntradasDisponibles;


            if (evento.AforoTotal < entradasVendidas)
            {
                return new ResultadoOperacion(
                    false,
                    "El aforo no puede ser menor que la cantidad " +
                    "de entradas que ya fueron vendidas.");
            }

            eventoExistente.AforoTotal =
                evento.AforoTotal;

            eventoExistente.EntradasDisponibles =
                evento.AforoTotal - entradasVendidas;

            eventoExistente.Activo =
                evento.Activo;

            try
            {
                _eventoRepository.Actualizar(
                    eventoExistente);

                _eventoRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "El evento fue actualizado correctamente.",
                    eventoExistente);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al actualizar el evento.");
            }
        }


        public ResultadoOperacion Desactivar(
            int eventoId)
        {
            if (eventoId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "El identificador del evento no es válido.");
            }

            Evento evento =
                _eventoRepository.ObtenerPorId(eventoId);

            if (evento == null)
            {
                return new ResultadoOperacion(
                    false,
                    "El evento indicado no existe.");
            }

            if (!evento.Activo)
            {
                return new ResultadoOperacion(
                    false,
                    "El evento ya se encuentra inactivo.");
            }

            evento.Activo = false;

            try
            {
                _eventoRepository.Actualizar(evento);
                _eventoRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "El evento fue desactivado correctamente.",
                    evento);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al desactivar el evento.");
            }
        }


        private ResultadoOperacion ValidarDatosGenerales(
            Evento evento)
        {
            if (evento == null)
            {
                return new ResultadoOperacion(
                    false,
                    "Debe proporcionar los datos del evento.");
            }

            if (string.IsNullOrWhiteSpace(
                evento.CodigoEvento))
            {
                return new ResultadoOperacion(
                    false,
                    "El código del evento es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                evento.Nombre))
            {
                return new ResultadoOperacion(
                    false,
                    "El nombre del evento es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                evento.Descripcion))
            {
                return new ResultadoOperacion(
                    false,
                    "La descripción del evento es obligatoria.");
            }

            if (evento.CategoriaEventoId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "Debe seleccionar una categoría.");
            }

            if (string.IsNullOrWhiteSpace(
                evento.Lugar))
            {
                return new ResultadoOperacion(
                    false,
                    "El lugar del evento es obligatorio.");
            }

            if (evento.PrecioEntrada < 0)
            {
                return new ResultadoOperacion(
                    false,
                    "El precio de la entrada no puede ser negativo.");
            }

            if (evento.AforoTotal <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "El aforo total debe ser mayor que cero.");
            }

            return new ResultadoOperacion(
                true,
                "Los datos del evento son válidos.");
        }


        private void NormalizarTextos(Evento evento)
        {
            evento.CodigoEvento =
                evento.CodigoEvento
                    .Trim()
                    .ToUpperInvariant();

            evento.Nombre =
                evento.Nombre.Trim();

            evento.Descripcion =
                evento.Descripcion.Trim();

            evento.Lugar =
                evento.Lugar.Trim();
        }


        public void Dispose()
        {
            _eventoRepository.Dispose();
        }
    }
}
