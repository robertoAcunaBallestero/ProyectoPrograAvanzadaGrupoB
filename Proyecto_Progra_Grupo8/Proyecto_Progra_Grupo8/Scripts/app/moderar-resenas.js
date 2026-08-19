$(function () {
    $(".btn-aprobar-resena").on("click", function () {
        moderarResena($(this), $(this).data("resenaId"), "Aprobada");
    });

    $(".btn-rechazar-resena").on("click", function () {
        moderarResena($(this), $(this).data("resenaId"), "Rechazada");
    });
});

function moderarResena($boton, resenaId, estado) {
    var $spinner = $boton.find(".spinner-border");

    $boton.prop("disabled", true);
    $spinner.removeClass("d-none");

    $.ajax({
        url: "/api/resenas/" + resenaId,
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify({ estado: estado }),
        dataType: "json"
    })
        .done(function (datos) {
            mostrarToast(datos.mensaje, "success");

            var $modal = $boton.closest(".modal");

            if ($modal.length) {
                var instancia = bootstrap.Modal.getInstance($modal[0]);

                if (instancia) {
                    instancia.hide();
                }
            }

            $("#fila-resena-" + resenaId).remove();

            var $tabla = $("#tablaResenas");

            if ($tabla.length && $tabla.find("tbody tr").length === 0) {
                $tabla.closest(".table-responsive").addClass("d-none");
                $("#moderarSinPendientes").removeClass("d-none");
            }
        })
        .fail(function (jqXHR) {
            var mensaje = "No se pudo actualizar la reseña.";

            if (jqXHR.responseJSON && jqXHR.responseJSON.mensaje) {
                mensaje = jqXHR.responseJSON.mensaje;
            }

            mostrarToast(mensaje, "error");
        })
        .always(function () {
            $boton.prop("disabled", false);
            $spinner.addClass("d-none");
        });
}
