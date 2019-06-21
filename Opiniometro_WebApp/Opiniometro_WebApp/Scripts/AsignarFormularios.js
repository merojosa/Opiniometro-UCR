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

/*$(document).ready(function () {
    document.getElementById("boton-efectuar").addEventListener("click",
        function () {
            document.getElementById("efectuar").click();
        });
});/**/
/**/
$(document).ready(function () {
    document.getElementById("boton-efectuar").addEventListener("click",
        function () {
            var filas = document.getElementsByClassName("fila-formulario");

            var FormulariosYPeriodos = [];

            var datosFila = {
                CodigoForm : ' ',
                FechaInicio : ' ',
                FechaFinal : ' '
            };

            /*for (var fila = 0; fila < filas.length; fila++) {
                datosFila.CodigoForm = filas[fila].getElementById("codigo-form").val();
                datosFila.FechaInicio = filas[fila].getElementById("fecha-inicio").val();
                datosFila.FechaFinal = filas[fila].getElementById("fecha-final").val();

                FormulariosYPeriodos.push(datosFila);
            };*/

            $.post("MetodoPrueba",
                {
                    PeriodosIndicados: FormulariosYPeriodos
                },
                function (data, status) {
                    /*alert("FUNCIONA? :O");*/
                }
            );
        });
});/**/