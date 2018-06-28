$(function () {
    var ddlAfiliado = $('#ID_Afiliado');
    var ddlTurno = $('#ID_Turno');

    ddlAfiliado.on('change', function () {
        var ID = $(this).val();
        ddlTurno.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlTurno);
        if (ID) {
            $.getJSON('/Turnos/ObtenerTurnosPorAfiliado/', { ID_Afiliado: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlTurno);
                });
            });
        }
    });
});