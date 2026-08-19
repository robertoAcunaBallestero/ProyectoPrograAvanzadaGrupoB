$(function () {
    var $spinner = $("#dashboardSpinner");
    var $error = $("#dashboardError");
    var $contenido = $("#dashboardContenido");

    $.ajax({
        url: "/api/dashboard",
        method: "GET",
        dataType: "json"
    })
        .done(function (datos) {
            renderizarDashboard(datos);
            $spinner.addClass("d-none");
            $contenido.removeClass("d-none");
        })
        .fail(function (jqXHR) {
            var mensaje = "No se pudieron cargar las estadísticas del dashboard.";

            if (jqXHR.responseJSON && jqXHR.responseJSON.mensaje) {
                mensaje = jqXHR.responseJSON.mensaje;
            }

            $spinner.addClass("d-none");
            $error.text(mensaje).removeClass("d-none");
            mostrarToast(mensaje, "error");
        });
});

function renderizarDashboard(datos) {
    $("#totalUsuarios").text(datos.totalUsuarios);
    $("#totalEventosActivos").text(datos.totalEventosActivos);
    $("#totalEventosProximos").text(datos.totalEventosProximos);
    $("#totalOrdenes").text(datos.totalOrdenes);
    $("#entradasVendidas").text(datos.entradasVendidas);

    $("#ingresosTotales").text(
        "₡" + Number(datos.ingresosTotales).toLocaleString(
            "es-CR",
            { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

    renderizarBajoAforo(datos.eventosBajoAforo || []);
    renderizarGrafico(datos.ingresosPorEvento || {});
}

function renderizarBajoAforo(eventos) {
    var $vacio = $("#bajoAforoVacio");
    var $tabla = $("#bajoAforoTabla");
    var $body = $("#bajoAforoBody");

    $body.empty();

    if (!eventos.length) {
        $vacio.removeClass("d-none");
        $tabla.addClass("d-none");
        return;
    }

    $vacio.addClass("d-none");
    $tabla.removeClass("d-none");

    $.each(eventos, function (indice, evento) {
        var $fila = $("<tr></tr>");

        var $celdaEvento = $("<td></td>");

        $("<strong></strong>")
            .text(evento.nombre)
            .appendTo($celdaEvento);

        $("<div class='small text-muted'></div>")
            .text(new Date(evento.fechaHora).toLocaleDateString("es-CR"))
            .appendTo($celdaEvento);

        var $celdaDisponibles = $("<td class='text-center'></td>");

        $("<span class='badge bg-danger'></span>")
            .text(evento.entradasDisponibles)
            .appendTo($celdaDisponibles);

        $("<div class='small text-muted mt-1'></div>")
            .text("de " + evento.aforoTotal)
            .appendTo($celdaDisponibles);

        $fila.append($celdaEvento, $celdaDisponibles);
        $body.append($fila);
    });
}

function renderizarGrafico(ingresosPorEvento) {
    var $canvas = $("#graficoIngresos");
    var $vacio = $("#graficoVacio");

    var etiquetas = Object.keys(ingresosPorEvento);

    if (!etiquetas.length) {
        $canvas.addClass("d-none");
        $vacio.removeClass("d-none");
        return;
    }

    var valores = $.map(etiquetas, function (clave) {
        return ingresosPorEvento[clave];
    });

    new Chart($canvas[0].getContext("2d"), {
        type: "bar",

        data: {
            labels: etiquetas,
            datasets: [
                {
                    label: "Ingresos",
                    data: valores
                }
            ]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,

            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return "₡" + value.toLocaleString();
                        }
                    }
                }
            },

            plugins: {
                legend: {
                    display: false
                },

                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return "₡" + context.raw.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}
