$(function () {
    $('.radiobuttons').attr('name', 'groupbuttons');
    $("input[type='radio']").click(function () {
        var radioValue = $("input[name='groupbuttons']:checked").val();
        $('#estatusVehiculo').val(radioValue).trigger('change');
    });
    $('#estatusVehiculo').on('change', function () {
        $('form').submit();
    });
});