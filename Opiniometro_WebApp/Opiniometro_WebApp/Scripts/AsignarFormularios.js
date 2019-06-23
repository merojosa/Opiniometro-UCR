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

function ObtenerFormsYPeriodosAsignados() {
    var codigos = document.getElementsByClassName("codigo-form");
    var inicios = document.getElementsByClassName("fecha-inicial");
    var fines = document.getElementsByClassName("fecha-final");

    var FormulariosYPeriodos = [];

    for (var f = 0; f < codigos.length; f++) {
        FormulariosYPeriodos.push(
            {
                CodigoForm: codigos[f].value,
                FechaInicio: inicios[f].value,
                FechaFinal: fines[f].value
            });
    };
    return FormulariosYPeriodos;
}
function ObtenerGruposAsignados() {
    var annos = document.getElementsByClassName("anno-grupo");
    var semestres = document.getElementsByClassName("semestre-grupo");
    var siglas = document.getElementsByClassName("sigla-grupo");
    var numeros = document.getElementsByClassName("numero-grupo");

    var grupos = [];

    for (var g = 0; g < annos.length; g++) {
        grupos.push(
            {
                SiglaCurso: siglas[g].value,
                Numero: numeros[g].value,
                AnnoGrupo: annos[g].value,
                SemestreGrupo: semestres[g].value
            });
    };
    return grupos;

}

$(document).ready(function () {
    document.getElementById("boton-efectuar").addEventListener("click",
        function () {
            var codigos = document.getElementsByClassName("codigo-form");
            var inicios = document.getElementsByClassName("fecha-inicial");
            var fines = document.getElementsByClassName("fecha-final");

            var FormulariosYPeriodos = ObtenerFormsYPeriodosAsignados();
            var Grupos = ObtenerGruposAsignados();

            $.post("EfectuarAsignaciones",
                {
                    Grupos: JSON.stringify(Grupos),
                    PeriodosIndicados: JSON.stringify(FormulariosYPeriodos)
                },
                function (data, status) {
                    alert(`Post fue exitoso?\n${status}`);
                }
            );
        });
});