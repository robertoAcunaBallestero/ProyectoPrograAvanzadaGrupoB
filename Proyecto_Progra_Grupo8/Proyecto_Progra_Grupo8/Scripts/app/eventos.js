$(function () {
    var $codigo = $("#Evento_CodigoEvento");

    if ($codigo.length === 0) {
        return;
    }

    var $feedback = $("<div class='form-text'></div>").insertAfter($codigo);

    $codigo.on("blur", function () {
        var codigo = $codigo.val().trim();
        var eventoId = $("#Evento_EventoId").val() || 0;

        $feedback.text("").removeClass("text-danger text-success");
        $codigo.removeClass("is-invalid");

        if (!codigo) {
            return;
        }

        $.ajax({
            url: "/api/eventos/codigo-disponible",
            method: "GET",
            data: { codigo: codigo, eventoId: eventoId },
            dataType: "json"
        }).done(function (respuesta) {
            if (respuesta.disponible) {
                $feedback.text("Código disponible.").addClass("text-success");
            } else {
                $feedback.text("Este código ya está en uso.").addClass("text-danger");
                $codigo.addClass("is-invalid");
            }
        });
    });
});
