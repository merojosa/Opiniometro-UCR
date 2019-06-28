//Genera un vector con el rango específico del ítem.
function preguntaEscalar() {
    var inicio = document.getElementById('Inicio').value;
    var final = document.getElementById('Final').value;
    var estrellas = document.getElementById('Estrellas').value;

    var rango = [];

    for (var r = inicio; r < final.length; r++) {
        rango.push(
            {
                r
            });
    };
    return rango;
}

