$(function () {
    $('.tiposvehiculos-checks .dias').on('change', function () {
        var dias = $('.tiposvehiculos-checks .dias');
        var check = $('.tiposvehiculos-checks input.dias:checked');
        console.log(check);
        PopulateTV();
    }).change();
});

function PopulateTV() {
    var vals = $('.tiposvehiculos-checks input.dias:checked').map(function () {
        var val = $(this).prev('.tiposvehiculos-checks .ids').val();
        return val;
    }).get().join(',');
    $('#tiposvehiculos').val(vals);
}