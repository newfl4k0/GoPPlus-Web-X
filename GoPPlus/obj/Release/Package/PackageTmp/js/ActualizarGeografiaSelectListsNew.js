$(function () {
    var ddlEstado2 = $('#ID_EstadoCalonias');
    var ddlCiudad2 = $('#ID_CiudadCalonias');
    var ddlEstadoCalles = $('#ID_EstadoCalles');
    var ddlCiudadCalles = $('#ID_CiudadCalles');
    var ddlColoniaCalles = $('#ID_ColoniaCalles');
    
    ddlEstado2.on('change', function () {
        var ID = $(this).val();
        ActualizarCiudades_Calonias(ID, ddlCiudad2)
    });

    function ActualizarCiudades_Calonias(ID, ddlCiudad2) {
        ddlCiudad2.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlCiudad2);
        if (ID) {
            $.getJSON('/Ciudades/ObtenerCiudades/', { ID_Estado: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlCiudad2);
                });
            });
        }
        if (ddlCiudad2.length)
            ddlCiudad2.change();
    }
    // for calles

    ddlEstadoCalles.on('change', function () {
        var ID = $(this).val();
        ActualizarCiudades_Calles(ID, ddlCiudadCalles);
    })
    
    function ActualizarCiudades_Calles(ID, ddlCiudadCalles) {
        ddlCiudadCalles.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlCiudadCalles);
        if (ID) {
            $.getJSON('/Ciudades/ObtenerCiudades/', { ID_Estado: ID }, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo(ddlCiudadCalles);
                });
            })
        }
        //if (ddlCiudadCalles.length)
        //    ddlCiudadCalles.change();
    }

    ddlCiudadCalles.on('change', function () {
        var ID = $(this).val();
        ActualizarColonias_Calles(ID, ddlColoniaCalles);
    });

    function ActualizarColonias_Calles(ID, ddlColoniaCalles) {
        ddlColoniaCalles.empty();
        $(document.createElement('option')).attr('value', '').text('-Seleccione-').appendTo(ddlColoniaCalles);
        $.getJSON('/Colonias/ObtenerColonias/', { ID_Ciudad: ID }, function (result) {
            $(result).each(function () {
                $(document.createElement('option'))
                    .attr('value', this.Value)
                .text(this.Text)
                .appendTo(ddlColoniaCalles);
            });
        })
    }
});