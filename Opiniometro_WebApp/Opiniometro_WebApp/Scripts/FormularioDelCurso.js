

///recibe: --
//modifica: vista en las preguntas que tienen campode texto
//retorna--
function TextoLibre() {
      
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);

}

///recibe: --opciones que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen seleccion unica
//retorna--
function seleccionUnica(par1, campoObservacion) {

    if (campoObservacion == true) {



        //si la pregunta tiene campo de observacion
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);


        //en caso de que no tenga observacion, solo se muestra la seleccion unica
    }  
    var choosed = document.createElement("INPUT");
    choosed.setAttribute('type', 'radio');
    choosed.setAttribute('name', 'op2');
    //choosed.setAttribute('label', Opción1)

    selection.appendChild(choosed);
        //aqui va codigo de seleccion unica
}

///recibe:-- si tiene campo de observacion
//modifica: vista en las preguntas que tienen si o no
//retorna--
function SiNo(par1, campoObservacion) {

    if (campoObservacion == true) {

        //aqui va el codigo de Si-No


        //si la pregunta tiene campo de observacion
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);


        //en caso de que no tenga observacion, solo se muestra la Si-nO
    } else {

        //aqui va el codigo de Si-nO

    }

}

///recibe: --opciones que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen seleccion multiple
//retorna--
function seleccionMultiple(par1, campoObservacion) {

    if (campoObservacion == true) {

        //aqui va el codigo de seleccion multiple


        //si la pregunta tiene campo de observacion
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);


        //en caso de que no tenga observacion, solo se muestra la seleccion multiple
    } else {

        //aqui va el codigo de seleccion multiple

    }
}

///recibe: --rango de numeros que se muestran
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen respuesta de tipo escalar
//retorna--
function Escalar(par1, campoObservacion) {

    if (campoObservacion == true) {

        //aqui va el codigo de "escalar"


        //si la pregunta tiene campo de observacion
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);

        //en caso de que no tenga observacion, solo se muestra el "escalar"
    } else {

        //aqui va el codigo de "escalar"

    }
}

///recibe: 
//         --si tiene campo de observacion
//modifica: vista en las preguntas que tienen respouesta de estrella
//retorna--
function Estrella(par1, campoObservacion) {

    if (campoObservacion == true) {

        //aqui va el codigo de "estrella"


        //si la pregunta tiene campo de observacion
        var cont = document.getElementById('campoTexto');
        var elemento = document.createElement("INPUT");
        elemento.type = 'text';
        //elemento.setAttribute("size = 15 maxlength = 30");
        cont.appendChild(elemento);

        //en caso de que no tenga observacion, solo se muestra el "estrella"
    } else {

        //aqui va el codigo de "estrella"

    }
}