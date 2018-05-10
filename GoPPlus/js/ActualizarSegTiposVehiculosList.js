$(function () {
    $('.dias').on('change', function () {
        var tiposVehiculos = $('.dias');
        var check = $('input[class="dias"]:checked');
        console.log(check);
        Populate();
    }).change();
});

function Populate() {
    var vals = $('input[class="dias"]:checked').map(function () {
        var val = $(this).prev('.ids').val();
        return val;
    }).get().join(',');
    $('#seg_tiposVehiculos').val(vals);
}