using Proyecto_Progra_Grupo8.Datos.Repositories;
using Proyecto_Progra_Grupo8.Entidades.Models;
using Proyecto_Progra_Grupo8.Negocio.Resultado;
using System;
using System.Collections.Generic;

namespace Proyecto_Progra_Grupo8.Negocio.Services
{
    public class ResenaService : IResenaService
    {
        private readonly IResenaRepository _resenaRepository;

        public ResenaService(
            IResenaRepository resenaRepository)
        {
            if (resenaRepository == null)
            {
                throw new ArgumentNullException(
                    nameof(resenaRepository));
            }

            _resenaRepository = resenaRepository;
        }

        public IEnumerable<Resena> ObtenerPendientes()
        {
            return _resenaRepository.ObtenerPendientes();
        }

        public IEnumerable<Resena> ObtenerAprobadasPorEvento(
            int eventoId)
        {
            if (eventoId <= 0)
            {
                return new List<Resena>();
            }

            return _resenaRepository
                .ObtenerAprobadasPorEvento(eventoId);
        }

        public IEnumerable<Resena> ObtenerPorUsuario(
            string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return new List<Resena>();
            }

            return _resenaRepository
                .ObtenerPorUsuario(usuarioId);
        }

        public Resena ObtenerPorId(int resenaId)
        {
            if (resenaId <= 0)
            {
                return null;
            }

            return _resenaRepository
                .ObtenerPorId(resenaId);
        }

        public ResultadoOperacion Crear(
            int eventoId,
            string usuarioId,
            string comentario)
        {
            if (eventoId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "El evento indicado no es válido.");
            }

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return new ResultadoOperacion(
                    false,
                    "Debe iniciar sesión para enviar una reseña.");
            }

            if (string.IsNullOrWhiteSpace(comentario))
            {
                return new ResultadoOperacion(
                    false,
                    "El comentario es obligatorio.");
            }

            comentario = comentario.Trim();

            if (comentario.Length < 5)
            {
                return new ResultadoOperacion(
                    false,
                    "El comentario debe contener al menos 5 caracteres.");
            }

            if (comentario.Length > 1000)
            {
                return new ResultadoOperacion(
                    false,
                    "El comentario no puede superar los 1000 caracteres.");
            }

            bool comproEvento =
                _resenaRepository.UsuarioComproEvento(
                    eventoId,
                    usuarioId);

            if (!comproEvento)
            {
                return new ResultadoOperacion(
                    false,
                    "Solo puede reseñar eventos que haya comprado.");
            }

            bool existeResena =
                _resenaRepository.ExisteResena(
                    eventoId,
                    usuarioId);

            if (existeResena)
            {
                return new ResultadoOperacion(
                    false,
                    "Ya realizó una reseña para este evento.");
            }

            Resena resena = new Resena
            {
                EventoId = eventoId,
                UsuarioId = usuarioId,
                Comentario = comentario,
                Estado = "Pendiente",
                FechaResena = DateTime.Now
            };

            try
            {
                _resenaRepository.Agregar(resena);
                _resenaRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "La reseña fue enviada y está pendiente de aprobación.",
                    resena);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al guardar la reseña.");
            }
        }

        public ResultadoOperacion Aprobar(int resenaId)
        {
            if (resenaId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "La reseña indicada no es válida.");
            }

            Resena resena =
                _resenaRepository.ObtenerPorId(resenaId);

            if (resena == null)
            {
                return new ResultadoOperacion(
                    false,
                    "La reseña indicada no existe.");
            }

            resena.Estado = "Aprobada";

            try
            {
                _resenaRepository.Actualizar(resena);
                _resenaRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "La reseña fue aprobada correctamente.",
                    resena);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al aprobar la reseña.");
            }
        }

        public ResultadoOperacion Rechazar(int resenaId)
        {
            if (resenaId <= 0)
            {
                return new ResultadoOperacion(
                    false,
                    "La reseña indicada no es válida.");
            }

            Resena resena =
                _resenaRepository.ObtenerPorId(resenaId);

            if (resena == null)
            {
                return new ResultadoOperacion(
                    false,
                    "La reseña indicada no existe.");
            }

            resena.Estado = "Rechazada";

            try
            {
                _resenaRepository.Actualizar(resena);
                _resenaRepository.Guardar();

                return new ResultadoOperacion(
                    true,
                    "La reseña fue rechazada correctamente.",
                    resena);
            }
            catch (Exception)
            {
                return new ResultadoOperacion(
                    false,
                    "Ocurrió un error al rechazar la reseña.");
            }
        }

        public void Dispose()
        {
            _resenaRepository.Dispose();
        }
    }
}
