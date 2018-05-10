$(function () {
    var ddlMarca = $('#ID_Marca');
    var ddlModelo = $('#ID_Modelo');

    ddlMarca.on('change', function () {
        var ID = $(this).val();
        ddlModelo.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlModelo);
        if (ID) {
            $.getJSON('/Modelos/ObtenerModelos/', { ID_Marca: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlModelo);
                });
            });
        }
    });
});