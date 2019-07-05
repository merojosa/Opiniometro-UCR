﻿


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
function seleccionUnica(contSelUnica/*,listaOpciones*/) {
  
}

///recibe:-- si tiene campo de observacion
//modifica: vista en las preguntas que tienen si o no
//retorna--
function RespondeSiNo(contSiNo) {

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
function Escalar() {


}

///recibe: 
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen respouesta de estrella
//retorna--
function Estrella() {


}