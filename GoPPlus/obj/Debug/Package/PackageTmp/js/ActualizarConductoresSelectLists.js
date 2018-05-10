$(function () {
    var ddlFlota = $('#ID_Flota');
    var ddlTurno = $('#ID_Turno');

    ddlFlota.on('change', function () {
        var ID = $(this).val();
        ddlTurno.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlTurno);
        if (ID) {
            $.getJSON('/Turnos/ObtenerTurnos/', { ID_Flota: ID }, function (result) {
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