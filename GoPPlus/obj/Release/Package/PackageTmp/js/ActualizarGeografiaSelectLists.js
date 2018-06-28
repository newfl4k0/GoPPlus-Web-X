$(function () {
    var ddlEstado = $('#ID_Estado');
    var ddlCiudad = $('#ID_Ciudad');
    var ddlColonia = $('#ID_Colonia');
    var ddlCalle = $('#ID_Calle');
    var ddlEstadoOrigen = $('#ID_Estado_Origen');
    var ddlCiudadOrigen = $('#ID_Ciudad_Origen');
    var ddlColoniaOrigen = $('#ID_Colonia_Origen');
    var ddlCalleOrigen = $('#ID_Calle_Origen');
    var ddlEstadoDestino = $('#ID_Estado_Destino');
    var ddlCiudadDestino = $('#ID_Ciudad_Destino');
    var ddlColoniaDestino = $('#ID_Colonia_Destino');
    var ddlCalleDestino = $('#ID_Calle_Destino');

    ddlEstado.on('change', function () {
        var ID = $(this).val();
        ActualizarCiudades(ID, ddlCiudad)
    });
    ddlEstadoOrigen.on('change', function () {
        var ID = $(this).val();
        ActualizarCiudades(ID, ddlCiudadOrigen)
    });
    ddlEstadoDestino.on('change', function () {
        var ID = $(this).val();
        ActualizarCiudades(ID, ddlCiudadDestino)
    });

    function ActualizarCiudades(ID, ddl)
    {
        ddl.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddl);
        if (ID) {
            $.getJSON('/Ciudades/ObtenerCiudades/', { ID_Estado: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddl);
                });
            });
        }
        if (ddl.length)
            ddl.change();
    }
    ddlCiudad.on('change', function () {
        var ID = $(this).val();
        ActualizarColonias(ID, ddlColonia)
    });
    ddlCiudadOrigen.on('change', function () {
        var ID = $(this).val();
        ActualizarColonias(ID, ddlColoniaOrigen)
    });
    ddlCiudadDestino.on('change', function () {
        var ID = $(this).val();
        ActualizarColonias(ID, ddlColoniaDestino)
    });

    function ActualizarColonias(ID, ddl) {
        ddl.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddl);
        if (ID) {
            $.getJSON('/Colonias/ObtenerColonias/', { ID_Ciudad: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddl);
                });
            });
        }
        if (ddl.length)
            ddl.change();
    }

    ddlColonia.on('change', function () {
        var ID = $(this).val();
        ActualizarCalles(ID, ddlCalle);
    });
    ddlColoniaOrigen.on('change', function () {
        var ID = $(this).val();
        ActualizarCalles(ID, ddlCalleOrigen)
    });
    ddlColoniaDestino.on('change', function () {
        var ID = $(this).val();
        ActualizarCalles(ID, ddlCalleDestino)
    });

    function ActualizarCalles(ID, ddl) {
        ddl.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddl);
        if (ID) {
            $.getJSON('/Calles/ObtenerCalles/', { ID_Colonia: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddl);
                });
                //if (result != '') {
                //    var resText = result[0].Text;
                //    searchlocations(resText.trim().replace(/ /g, '%20'));
                //}
            });
        }
    }
});