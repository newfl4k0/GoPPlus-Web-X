$(function () {
    var ddlClienteAbonado = $('#ID_ClienteAbonado');
    var ddlUsuarioAbonado = $('#ID_UsuarioAbonado');

    ddlClienteAbonado.on('change', function () {
        var ID = $(this).val();
        ddlUsuarioAbonado.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlUsuarioAbonado);
        if (ID) {
            $.getJSON('/UsuariosAbonados/ObtenerUsuariosAbonados/', { ID_ClienteAbonado: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlUsuarioAbonado);
                });
            });
        }
    });
});