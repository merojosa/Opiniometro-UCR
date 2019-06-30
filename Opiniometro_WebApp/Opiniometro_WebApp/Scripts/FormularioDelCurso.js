


////modifica el id del div de las preguntas que poseen campo de texto libre
//function renombrar(num) {
//   $("#campoTexto0").attr("id", "campoTexto1");
//}

//function newLine(etiqueta) {
//    var elemento = document.createElement("hr/");
//    var f = document.getElementById(etiqueta);
//    document.getElementById(etiqueta).appendChild(elemento);
//}

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
    var elemento = document.createElement("INPUT");
    elemento.type = 'text';
    elemento.style.width = "400px";
    elemento.style.height = "150px";
    contenido.appendChild(elemento);
}

///recibe: --opciones que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen seleccion unica
//retorna--
function seleccionUnica(contSelUnica/*,listaOpciones*/) {

    var contenido = document.getElementById("selUnica".concat(contSelUnica));
    var choosed = document.createElement("INPUT");
    choosed.setAttribute('type', 'radio');
    choosed.setAttribute('name', contSelUnica);
    //choosed.setAttribute('label', Opción1)

    contenido.appendChild(choosed);
        //aqui va codigo de seleccion unica

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