


function escogerTipoRespuesta(tipoPregunta, tieneObservacion) {
    switch (tipoPregunta) {

        //HAY QUE VER QUE NUMERO LE ASIGNÓ JOFFI A CADA TIPO DE PREGUNTA PARA COLOCARLO EN LOS CASOS

        //caso 1: respuesta de texto libre
        case tipoPregunta = 1: 
                var cont = document.getElementById('campoTexto');
                var elemento = document.createElement("INPUT");
                elemento.type = 'text';
                cont.appendChild(elemento);                        
            break;    

        //caso 2: respuesta seleccion unica
        case tipoPregunta = 2:
            //si posee campo de observacion, entonces
            if (tieneObservacion == true) {

                //aqui va codigo de seleccion unica

                //la pregunta tambien incluye campo de observacion
                var cont = document.getElementById('campoTexto');
                var elemento = document.createElement("INPUT");
                elemento.type = 'text';
                cont.appendChild(elemento);

                //en caso de que no tenga observacion, solo se muestra la seleccion unica
            } else {
                var selection = document.getElementById('campoTexto'); //para guardar la respuesta?

                var choosed = document.createElement("INPUT");
                choosed.setAttribute('type', 'radio');
                choosed.setAttribute('name', 'op2');
                //choosed.setAttribute('label', Opción1)

                selection.appendChild(choosed);
                //aqui va codigo de seleccion unica
            }
            break;


        //respuesta seleccion multiple
        case tipoPregunta = 3:
             //si posee campo de observacion, entonces
            if (tieneObservacion == true) {

                //aqui va codigo de seleccion multiple

                //la pregunta tambien incluye campo de observacion
                var cont = document.getElementById('campoTexto');
                var elemento = document.createElement("INPUT");
                elemento.type = 'text';
                cont.appendChild(elemento);

                //en caso de que no tenga observacion, solo se muestra la seleccion multiple
            } else {

                //aqui va codigo de seleccion multiple

            }

            break;
    

        //caso 4: respuesta escalar
        case 4:
           //si posee campo de observacion, entonces
            if (tieneObservacion == true) {

                //aqui va codigo de escalar

                //la pregunta tambien incluye campo de observacion
                var cont = document.getElementById('campoTexto');
                var elemento = document.createElement("INPUT");
                elemento.type = 'text';
                cont.appendChild(elemento);

                //en caso de que no tenga observacion, solo se muestra la respuesta con escalares
            } else {
                 //aqui va codigo de escalar
            }
            break;

        default:
            break;

    }
}