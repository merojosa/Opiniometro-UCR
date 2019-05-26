function obtenerCntRespGrupo(codigoFormulario, cedulaProfesor, annoGrupo, semestreGrupo, numeroGrupo, siglaCurso, itemId, respuesta, resultado) {
    $.get("/Responde/ObtenerCantidadRespuestasPorPregunta", {
        cursoId: cursoId,
        codigoFormulario: codigoFormulario,
        cedulaProfesor: cedulaProfesor,
        annoGrupo: annoGrupo,
        semestreGrupo: semestreGrupo,
        numeroGrupo: numeroGrupo,
        siglaCurso: siglaCurso,
        itemId: itemId,
        respuesta: respuesta,
        resultado: resultado
    }, function (resultadoCntRespGrupo) {
        $('#cntResp').val(resultadoCntRespGrupo);
    });
}