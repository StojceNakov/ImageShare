$(document).ready(function () {

    $(".top-menu-items").hover(function () {

        $(this).addClass("top-menu-border");

    },
    function () {

        $(this).removeClass("top-menu-border");
    });

    $(".sidebar-menu-items").hover(function () {

        $(this).addClass("sidebar-menu-border");

    },
    function () {

        $(this).removeClass("sidebar-menu-border");
    });


    $(".blurImage").hover(function () {
        $(this).css('cursor', 'pointer');
        $(this).fadeTo(10, 0.5);

    },
    function () {
        $(this).css('cursor', '');
        $(this).fadeTo(10, 1);
    });

    $(".showText").hover(function () {

        $(this).find(".tekst").removeClass("invisible", 10, "easeOutBounce");
    },
    function () {

        $(this).find(".tekst").addClass("invisible", 10, "easeOutBounce");
    });
});