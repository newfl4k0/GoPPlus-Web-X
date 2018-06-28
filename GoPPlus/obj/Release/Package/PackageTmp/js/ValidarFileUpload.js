$(function () {
    $(".file_hidden").each(function () {
        if ($(this).val()) {
            var url = $(this).val().split('\\');
            var filenamecomplete = url[url.length - 1];
            var vectorextension = filenamecomplete.split('.');
            var vectorfilename = vectorextension[0].split('-');
            vectorfilename.length = vectorfilename.length - 1;
            var filename = Array.prototype.slice.call(vectorfilename).join("-");
            var extension = vectorextension[vectorextension.length - 1];
            var file = filename + "." + extension;
            if (file) {
                $(this).parent().find('.file_upload').removeClass('btn-default').addClass('btn-success').toggleClass('selected').parent().find('.file_name').html(file);
            }
        }
    });

    $('.show_file_image .close_image').on('click', function () {
        $(this).next('img').attr('src', '');
        $(this).parent().parent().find('.file_upload').find('input[type="file"]').val('').change();
        $(this).hide();
    });

    $('input[type="file"]').change(function () {
        if ($(this).val()) {
            var url = $(this).val().split('\\');
            var file = url[url.length-1];
            $(this).closest('.file_upload').removeClass('btn-default').addClass('btn-success').toggleClass('selected').parent().find('.file_name').html(file);
        }
        else
        {
            if ($(this).closest('.file_upload').hasClass('selected'))
                $(this).closest('.file_upload').removeClass('btn-success').addClass('btn-default').toggleClass('selected').parent().find('.file_name').html('');
        }
    });
});