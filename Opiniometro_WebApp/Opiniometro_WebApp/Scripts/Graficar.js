function graficarResultados(itemId) {
    alert($(this).attr(itemId));
    $.get("/VisualizarFormulario/GraficoPie", itemId);
}