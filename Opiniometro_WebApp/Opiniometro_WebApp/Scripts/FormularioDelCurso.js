

function pruebaLlamado(tieneObs) {
    if (tieneObs == true) {
        var n1 = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        //var contenido = document.createTextNode("type = text")
        //elemento.appendChild(contenido)
        elemento.type = 'text';
        n1.appendChild(elemento);
        //document.getElementById("campoTexto").appendChild(elemento);
    }
}