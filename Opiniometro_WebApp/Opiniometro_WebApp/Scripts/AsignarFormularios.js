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
            if (confirm("¿Seguro de que desea efectuar las asignaciones?")) {
                var botonEfectuar = document.getElementById("efectuar");
                botonEfectuar.disabled = true;

                var exitoso = false;

                var codigos = document.getElementsByClassName("codigo-form");
                var inicios = document.getElementsByClassName("fecha-inicial");
                var fines = document.getElementsByClassName("fecha-final");

                var FormulariosYPeriodos = ObtenerFormsYPeriodosAsignados();
                var Grupos = ObtenerGruposAsignados();

                $.ajax({
                    type: "POST",
                    url: "/AsignarFormularios/EfectuarAsignaciones",
                    data: {
                        Grupos: JSON.stringify(Grupos),
                        PeriodosIndicados: JSON.stringify(FormulariosYPeriodos)
                    },
                    success: function (data, status) {
                        var d = JSON.parse(data);
                        if (d == 0) {
                            alert("Asignaciones fallidas. Por favor revise el formato de los periodos seleccionados.");
                        }
                        else {
                            if (d == -1) {
                                alert("Hubo un problema con la base de datos. Por favor contacte a soporte técnico.");
                            }
                            else {
                                if (d < FormulariosYPeriodos.length * Grupos.length) {
                                    alert("Se ha realizado algunas asignaciones. Es posible que algunas fallaran. Por favor revise los periodos seleccionados y vuelva a intentarlo.")
                                }
                                else {
                                    alert("Se concretó exitosamente todas las asignaciones.");
                                }

                                window.location.href = "/Asignacion_Formulario/index";
                            }
                        }
                    },
                    error: function (data, status) {
                        alert(`Ha ocurrido un error. Por favor intente de nuevo.\nStatus: ${status}`);
                        botonEfectuar.disabled = false;
                    }
                });
            }
            else {
                // No hacer nada: se cancela la asignación.
            }
        });
});
