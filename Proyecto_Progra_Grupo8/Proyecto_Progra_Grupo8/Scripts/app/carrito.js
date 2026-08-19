$(function () {
    var $formAgregar = $("#formAgregarCarrito");

    if ($formAgregar.length) {
        $formAgregar.on("submit", function (evento) {
            evento.preventDefault();
            agregarAlCarrito($formAgregar);
        });
    }

    var $btnConfirmarCompra = $("#btnConfirmarCompra");

    if ($btnConfirmarCompra.length) {
        $btnConfirmarCompra.on("click", function () {
            confirmarCompra($btnConfirmarCompra);
        });
    }
});

function agregarAlCarrito($form) {
    var $boton = $("#btnAgregarCarrito");
    var $spinner = $boton.find(".spinner-border");
    var cantidad = $form.find("#Cantidad").val();

    $boton.prop("disabled", true);
    $spinner.removeClass("d-none");

    $.ajax({
        url: "/api/carrito/agregar",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            eventoId: parseInt($form.data("eventoId"), 10),
            cantidad: parseInt(cantidad, 10)
        }),
        dataType: "json"
    })
        .done(function (datos) {
            mostrarToast(datos.mensaje, "success");
        })
        .fail(function (jqXHR) {
            var mensaje = "No se pudo agregar al carrito.";

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

function confirmarCompra($boton) {
    var $spinner = $boton.find(".spinner-border");

    $boton.prop("disabled", true);
    $spinner.removeClass("d-none");

    $.ajax({
        url: "/api/carrito/comprar",
        method: "POST",
        dataType: "json"
    })
        .done(function (datos) {
            mostrarToast(datos.mensaje, "success");

            var modalEl = document.getElementById("modalConfirmarCompra");

            if (modalEl) {
                var instancia = bootstrap.Modal.getInstance(modalEl);

                if (instancia) {
                    instancia.hide();
                }
            }

            window.location.href = "/Ordenes/Detalle/" + datos.ordenId;
        })
        .fail(function (jqXHR) {
            var mensaje = "No se pudo confirmar la compra.";

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
