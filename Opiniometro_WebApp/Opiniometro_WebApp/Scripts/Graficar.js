function graficarResultados(itemId) {
    alert(itemId);
    $.get("/VisualizarFormulario/GraficoPie", {itemId});
}