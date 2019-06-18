function cambioUnidadSelecionada(codigoUnidad) {
    $.get("/AsignarFormularios/ObtenerCarreras", {
        codigoUnidad: codigoUnidad
    });
    cambioCarreraSelecionada(codigoUnidad)
}

function cambioCarreraSelecionada(codigoUnidad) {
    $.get("/AsignarFormularios/ObtenerCursos", {
        codigoUnidad: codigoUnidad
    });
}

$(document).ready(function () {
    document.getElementById("boton-asignar").addEventListener("click",
        function () {
            document.getElementById("boton-abrir-modal").click();
            //alert("CLICKED!");
        });
});