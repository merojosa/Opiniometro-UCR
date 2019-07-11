///Se crea elemento que agraga un salto de linea

function insertNueLinea(etiqueta) {
    var elemento = document.createElement("br");
    document.getElementById(etiqueta).appendChild(elemento);
}

function lineaHor(etiqueta) {
    var elemento = document.createElement("hr");
    document.getElementById(etiqueta).appendChild(elemento);
}

//crear cada elemento que contendra la pregunta con su id UNICO
function crearElemento(nombreId, numeroId, pregunta, etiquetaDiv) {
    var elemento = document.createElement("div");
    var contenido = document.createTextNode(pregunta);
    elemento.appendChild(contenido);
    elemento.setAttribute("id", nombreId.concat(numeroId));
    document.getElementById(etiquetaDiv).appendChild(elemento);
    //document.body.appendChild(elemento);
}
 
///recibe: --
//modifica: vista en las preguntas que tienen campo de texto
//retorna--
function TextoLibre(contTextoLibre) {
    var contenido = document.getElementById("campoTexto".concat(contTextoLibre)) ;
    var elemento = document.createElement("textarea");
    elemento.rows = "5";
    elemento.cols = "50";
    elemento.maxLength = "160";   
    contenido.appendChild(elemento);
}

///recibe: --opciones que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen seleccion unica
//retorna--
function seleccionUnica(contSelUnica, item_id) {

    var contenido = document.getElementById("preguntaSelUnica".concat(contSelUnica));
    /*
    $.get("/FormularioDelCurso/ObtenerOpcionesSelUnica", {
        id: item_id
    }, function (json_list) {
        alert(json_list);

        var list = JSON.parse(json_list);
        foreach (var op in list)
        {
            var radio = document.createElement("input");
            radio.type = "radio";
            radio.name = op;
            radio.id = op.concat(contSelUnica);
            radio.value = "op";
            contenido.appendChild(radio);
        }
});*/


}

///recibe:-- si tiene campo de observacion
//modifica: vista en las preguntas que tienen si o no
//retorna--
function RespondeSiNo(contSiNo) {
    var x = document.createElement("INPUT");
    x.setAttribute("type", "radio");
    contenido.appendChild(x);
}


///recibe: --opciones que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen seleccion multiple
//retorna--
function seleccionMultiple() {

  
}

///recibe: --rango de numeros que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen respuesta de tipo escalar
//retorna--
//function Escalar(contEscalar, item_id) {
//    var contenido = document.getElementById("preguntaEscalar".concat(contEscalar));

    //    $.get("/FormularioDelCurso/ObtenerOpcionEscalar", {
    //        id: item_id
    //    }, function (json_list) {
    //        alert(json_list);

    //        var list = JSON.parse(json_list);
    //        foreach (var op in list)
    //        {
    //            var radio = document.createElement("input");
    //            radio.type = "radio";
    //            radio.name = op;
    //            radio.id = op.concat(contEscalar);
    //            radio.value = "op";
    //            contenido.appendChild(radio);
    //        }
    //});
//}

//recibe: si tiene campo de observacion
//modifica: vista en las preguntas que tienen respouesta de estrella
//retorna:
function Estrella() {


}

function recolectarRespuestas()
{
    var cedEst = document.getElementsByClassName("cedEstudiante")[0].textContent;
    var cedProf = document.getElementsByClassName("cedProfesor")[0].textContent;
    var codigoFormulario = document.getElementsByClassName("codFormulario")[0].textContent;

    var datosGrupo = document.getElementsByClassName("datos-grupo");
    var grupo = {
        Anno: datosGrupo[0].textContent,
        Semestre: datosGrupo[1].textContent,
        SiglaCurso: datosGrupo[2].textContent,
        NumeroGrupo: datosGrupo[3].textContent
    }
    var respuestasFormulario = [];

    //alert(`Profesor: ${cedProf}`);
    //alert(`Enviando respuestas para: ${grupo.Anno} ${grupo.Semestre} ${grupo.SiglaCurso} ${grupo.NumeroGrupo}`);

    var secciones = document.getElementsByClassName("seccion");
    for (var s = 0; s < secciones.length; s++) {
        var titulo = secciones[s].getElementsByClassName("titulo")[0].textContent;

        var preguntas = secciones[s].getElementsByClassName("pregunta");
        for (var p = 0; p < preguntas.length; p++) {

            var tipo = preguntas[p].getElementsByClassName("tipo")[0].textContent;
            //alert(tipo);
            var idBD = preguntas[p].getElementsByClassName("id-bd")[0].textContent;
            var respuestas = [];
            if (tipo == 1) {
                respuestas.push(preguntas[p].getElementsByClassName("respuesta")[0].value);
                //alert(respuestaLibre);
            } else if (tipo == 2) {
                var radios = preguntas[p].getElementsByClassName("respuesta");
                var contenidos = preguntas[p].getElementsByClassName("contenido-opcion");
                for (var r = 0; r < radios.length; r++) {
                    if (radios[r].checked) {
                        respuestas.push(contenidos[r].textContent);
                        //alert(contenidos[r].textContent);
                    }
                }
            } else if (tipo == 3) {
                var radios = preguntas[p].getElementsByClassName("respuesta");
                var contenidos = preguntas[p].getElementsByClassName("contenido-opcion");
                for (var r = 0; r < radios.length; r++) {
                    if (radios[r].checked) {
                        respuestas.push(contenidos[r].textContent);
                        //alert(contenidos[r].textContent);
                    }
                }
            } else if (tipo == 4) {
                var checks = preguntas[p].getElementsByClassName("respuesta");
                var contenidos = preguntas[p].getElementsByClassName("contenido-opcion");
                for (var r = 0; r < checks.length; r++) {
                    if (checks[r].checked) {
                        respuestas.push(contenidos[r].textContent);
                        //alert(contenidos[r].textContent);
                    }
                }
            } else if (tipo == 5) {
                var checks = preguntas[p].getElementsByClassName("respuesta");
                var contenidos = preguntas[p].getElementsByClassName("contenido-opcion");
                for (var r = 0; r < checks.length; r++) {
                    if (checks[r].checked) {
                        respuestas.push(contenidos[r].textContent);
                        //alert(contenidos[r].textContent);
                    }
                }
            } else if (tipo == 6) {

            }

            var hayObs = document.getElementsByClassName("bool-observacion")[0].textContent;
            var obs = "";
            if (hayObs == 'true') {
                obs = document.getElementsByClassName("observacion")[0].textContent;
                alert(`Jeje ${obs}`);
            }
            respuestasFormulario.push({ idItem: idBD, TituloSeccion: titulo, HilerasDeRespuesta: respuestas, Observacion: obs });

        } // for (var p = 0; p < preguntas.length; p++)
        
    } // for (var s = 0; s < secciones.length; s++)
    $.post("FormularioCurso/GuardarRespuestas",
        {
            CedulaEstudiante: JSON.stringify(cedEst),
            CedulaProfesor: JSON.stringify(cedProf),
            Grupo: JSON.stringify(grupo),
            CodigoFormulario: JSON.stringify(codigoFormulario),
            Respuestas: JSON.stringify(respuestasFormulario)
        },
        function (data, status) {
            alert("Gracias por brindar su respuesta.");
            window.location.href = "/CursosMatriculados/Index";
        }
    );

}