$(function () {
    $('#ID_Empresa').on('change', function () {
        var ID = $(this).val();
        ID = ID ? ID : "0";
        $.getJSON('/Afiliados/ObtenerNombreRFCPorEmpresa/', { ID_Empresa: ID }, function (result) {
            if (result.count > 0) {
                $("#Nombre").attr("readonly", "readonly");
                $("#RFC").attr("readonly", "readonly");
            }
            else {
                $("#Nombre").removeAttr("readonly");
                $("#RFC").removeAttr("readonly");
            }
            $("#Nombre").val(result.nombre);
            $("#RFC").val(result.rfc);
        });
    });
})