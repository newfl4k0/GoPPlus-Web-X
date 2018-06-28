$(function () {
    var ddlEmpresa = $('#ID_Empresa');
    var ddlAfiliado = $('#ID_Afiliado');

    ddlEmpresa.on('change', function () {
        var ID = $(this).val();
        ddlAfiliado.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlAfiliado);
        if (ID) {
            $.getJSON('/Afiliados/ObtenerAfiliados/', { ID_Empresa: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlAfiliado);
                });
            });
        }
    });
});