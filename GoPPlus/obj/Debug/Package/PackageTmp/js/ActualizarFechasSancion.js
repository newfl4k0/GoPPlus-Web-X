$(document).ready(function () {

    $("#Fecha_Inicio").val($("#Fecha_Inicio").val().split(" ")[0]);

    $("form").submit(function (event) {
        if (ValidateTime($("#Hora_Inicio").val())) {
            ConcatDateTime();
            return;
        }

        event.preventDefault();
    });

    $("#Hora_Inicio").on('input propertychange paste', function () {
        //ConcatDateTime();
        GetFechaFin();
    });

    $("#Fecha_Inicio").on('change', function () {
        //ConcatDateTime();
        GetFechaFin();
    });

    $("#ID_TipoSancion").on('change', function () {
        GetFechaFin();
    });
});

function GetFechaFin() {
    var ID = $("#ID_TipoSancion").val();
    var fecha_inicio = $("#Fecha_Inicio").val();
    var hora_inicio = CompleteHour($("#Hora_Inicio").val());
    hora_inicio = ValidateTime(hora_inicio) ? hora_inicio : "00:00";
    var fechahora_inicio = fecha_inicio + ' ' + hora_inicio;
    if (ID && fechahora_inicio) {
        $.getJSON('/Sanciones/ObtenerHoraFinSancion/', { ID_TipoSancion: ID, Fecha_Inicio: fechahora_inicio }, function (result) {
            if (result != null) {
                $("#Fecha_Fin").val(result);
            }
            else {
                $("#Fecha_Fin").val("");
            }
        });
    }
    else {
        $("#Fecha_Fin").val("");
    }
}

function ConcatDateTime() {
    var hora = CompleteHour($("#Hora_Inicio").val());
    var fecha = $("#Fecha_Inicio").val().split(' ')[0];
    if (fecha) {
        hora = ValidateTime(hora) ? hora : "00:00";
        $("#Fecha_Inicio").val(fecha + ' ' + hora);
    }
}

function ValidateTime(hour) {
    var valid = true;
    $("#Hora_Inicio").removeClass('error');
    var timeRegex = /^(?:[0-1]?[0-9]|2[0-3])(?::[0-5][0-9])?$/;
    if (!timeRegex.test(hour)) {
        valid = false;
        $("#Hora_Inicio").addClass('error');
    }
    return valid;
}

function CompleteHour(hour) {
    var vec = hour.split(":");
    var complete = vec.length > 1 ? hour : hour + ":00";
    return complete;
}