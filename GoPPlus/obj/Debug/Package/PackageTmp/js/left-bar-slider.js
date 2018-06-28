$('#sidebarbtn').click(function () {
    var resizedWindowWidth = $(window).width();
    if ($('#sidebar-collapse').css('display') == 'none') {
       // $('#sidebar-collapse').slideDown("slow");
        $('#sidebar-collapse').show();
        initializeSliderWidth(resizedWindowWidth);
    }
    else {
        //$('#sidebar-collapse').slideUp("slow");
        $('#sidebar-collapse').hide();
        //$('#main-content').css('margin-left', '0px');
        $('#main-content').removeClass();
        $('#main-content').addClass('col-sm-12 col-lg-12');
    }
});

$(window).resize(function () {
    var resizedWindowWidth = $(window).width();
    initializeSliderWidth(resizedWindowWidth)
});

function initializeSliderWidth(WindowWidth) {
    if (WindowWidth > 769) {
        $('#main-content').removeClass();
        $('#main-content').addClass('col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2');
        //$('#main-content').css('margin-left', '240px');
    } else {
        //$('#main-content').css('margin-left', '0px');
        $('#main-content').removeClass();
        $('#main-content').addClass('col-sm-12 col-lg-12');
    }
}
