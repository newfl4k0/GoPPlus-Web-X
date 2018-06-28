$(function () {
    $('.estatus-checks .dias').on('change', function () {
        var dias = $('.estatus-checks .dias');
        var check = $('.estatus-checks input.dias:checked');
        console.log(check);
        Populate();
    }).change();
});

function Populate() {
    var vals = $('.estatus-checks input.dias:checked').map(function () {
        var val = $(this).prev('.estatus-checks .ids').val();
        return val;
    }).get().join(',');
    $('#estatus').val(vals);
}