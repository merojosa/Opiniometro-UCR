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
            var codigos = document.getElementsByClassName("codigo-form");
            var inicios = document.getElementsByClassName("fecha-inicial");
            var fines = document.getElementsByClassName("fecha-final");

            var FormulariosYPeriodos = [];

            var datosFila = {
                CodigoForm : ' ',
                FechaInicio : ' ',
                FechaFinal : ' '
            };

            //alert(`${codigos.length} forms detectados\n${inicios.length} fechas i detectadas\n${fines.length} fechas f detectadas`);

            for (var f = 0; f < codigos.length; f++) {
                datosFila.CodigoForm = codigos[f].val;
                datosFila.FechaInicio = inicios[f].val;
                datosFila.FechaFinal = fines[f].val;

                FormulariosYPeriodos.push(datosFila);
            };

            var tablaPeriodos = document.getElementById("tabla-periodos");

            $.post("MetodoPrueba",
                {
                    PeriodosIndicados: FormulariosYPeriodos
                },
                function (data, status) {
                    alert("Post fue exitoso.");
                }
            );
        });
});/**/