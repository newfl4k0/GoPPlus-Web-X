$(function () {
    var previous;

    $(".vehiculosDDL").on('focus', function () {
        previous = this.value;
    }).change(function() {
        var IDddl = $(this).attr("id");
        var value = $(this).val();
        var newSel = $(".vehiculosDDL option[value='" + value + "']").not("#" + IDddl + " option[value='" + value + "']").not(".vehiculosDDL option[value='']");
        var oldSel = $(".vehiculosDDL option[value='" + previous + "']");
        newSel.hide();
        oldSel.show();
    });

    $(".vehiculosDDL").each(function () {
        var value = $(this).val();
        if(value !== "")
            $(".vehiculosDDL option[value='" + value + "']").not("#" + $(this).attr("id") + " option[value='" + value + "']").hide();
    });

    
});