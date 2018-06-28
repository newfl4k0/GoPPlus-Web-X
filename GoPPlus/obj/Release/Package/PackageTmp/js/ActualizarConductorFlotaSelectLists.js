$(function () {
    var ddlFlota = $('#ID_Flota');
    var ddlConductor = $('#ID_Conductor');

    ddlFlota.on('change', function () {
        var ID = $(this).val();
        ddlConductor.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlConductor);
        if (ID) {
            $.getJSON('/Conductores/ObtenerConductores/', { ID_Flota: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlConductor);
                });
            });
        }
    });
});