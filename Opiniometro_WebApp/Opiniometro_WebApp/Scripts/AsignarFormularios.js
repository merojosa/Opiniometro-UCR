function cambioUnidadSelecionada(codigoUnidad) {
    ObtenerCarreras(codigoUnidad);
    ObtenerCursos(codigoUnidad);
}

function obtenerUnidad(cursoId) {
    $.get("/CursoDetalles/ObtenerCantidadEstudiantes", {
        cursoId: cursoId
    }, function (resultadoCantidadEstudiantes) {
        $('#cantidadEstudiantes').val(resultadoCantidadEstudiantes);
    });
}

function obtenerCarrera(cursoId) {
    $.get("/CursoDetalles/ObtenerCantidadEstudiantes", {
        cursoId: cursoId
    }, function (resultadoCantidadEstudiantes) {
        $('#cantidadEstudiantes').val(resultadoCantidadEstudiantes);
    });
}

function obtenerCursos(cursoId) {
    $.get("/CursoDetalles/ObtenerCantidadEstudiantes", {
        cursoId: cursoId
    }, function (resultadoCantidadEstudiantes) {
        $('#cantidadEstudiantes').val(resultadoCantidadEstudiantes);
    });
}

function cambioCarreraSelecionada(cursoId) {
    obtenerCantidadEstudiantes(cursoId);
}

function obtenerCursos(cursoId) {
    $.get("/CursoDetalles/ObtenerCantidadEstudiantes", {
        cursoId: cursoId
    }, function (resultadoCantidadEstudiantes) {
        $('#cantidadEstudiantes').val(resultadoCantidadEstudiantes);
    });
}