// Helper compartido para mostrar notificaciones Bootstrap Toast
// desde cualquier vista que consuma la Web API vía AJAX.
function mostrarToast(mensaje, tipo) {
    var contenedor = document.getElementById("toastContainer");

    if (!contenedor) {
        return;
    }

    var colorClase = "text-bg-success";

    if (tipo === "error") {
        colorClase = "text-bg-danger";
    } else if (tipo === "warning") {
        colorClase = "text-bg-warning";
    }

    var toastEl = document.createElement("div");
    toastEl.className = "toast align-items-center " + colorClase + " border-0";
    toastEl.setAttribute("role", "alert");
    toastEl.setAttribute("aria-live", "assertive");
    toastEl.setAttribute("aria-atomic", "true");

    toastEl.innerHTML =
        '<div class="d-flex">' +
        '<div class="toast-body"></div>' +
        '<button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Cerrar"></button>' +
        "</div>";

    toastEl.querySelector(".toast-body").textContent = mensaje;

    contenedor.appendChild(toastEl);

    var toast = new bootstrap.Toast(toastEl, { delay: 4000 });

    toastEl.addEventListener("hidden.bs.toast", function () {
        toastEl.remove();
    });

    toast.show();
}
