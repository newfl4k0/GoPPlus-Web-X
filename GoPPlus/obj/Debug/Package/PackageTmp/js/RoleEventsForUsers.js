/// <reference path="RoleEventsForUsers.js" />
$(function () {
    $(".afiliados_div").hide();
    var MostrarAfiliados = $('#MostrarAfiliados').val() == "True";
    ShowByRole();
    $('#role').on('change', function () {
        ShowByRole();
    });
    function ShowByRole() {
        var rol = $('#role').val();
        if (rol) {
            $.getJSON('/Account/ShowPictureAndAfiliados/', { role: rol, mostrar: MostrarAfiliados }, function (result) {
                if (result.showPic)
                    $(".pic").show();
                else
                    $(".pic").hide();
                if (result.showAfi) {
                    if (result.radioButtons) {
                        $(".afiliados_radiobuttons").show();
                        $(".afiliados_checkboxes").hide();
                    }
                    else {
                        $(".afiliados_radiobuttons").hide();
                        $(".afiliados_checkboxes").show();
                    }
                    $(".afiliados_div").show();
                }
                else
                    $(".afiliados_div").hide();
            });
        }
        else {
            $(".pic").hide();
            $(".afiliados_div").hide();
        }
    }

    $('.roles_radiobuttons .radiobuttons').attr('name', 'groupbuttons');
    $(".roles_radiobuttons input[type='radio']").click(function () {
        var radioValue = $("input[name='groupbuttons']:checked").val();
        $('#role').val(radioValue).trigger('change');
    });

    $('.afiliados_radiobuttons .radiobuttons').attr('name', 'afiliadosbuttons');
    $(".afiliados_radiobuttons input[type='radio']").click(function () {
        var radioValue = $("input[name='afiliadosbuttons']:checked").val();
        $('#afiliados').val(radioValue).trigger('change');
    });
})