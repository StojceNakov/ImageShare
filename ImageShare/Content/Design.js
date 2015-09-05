$(document).ready(function()
{
    $(".top-menu-items").hover(function () {

        $(this).addClass("top-menu-border");

    },
    function () {

        $(this).removeClass("top-menu-border");
    })

    $(".sidebar-menu-items").hover(function () {

        $(this).addClass("sidebar-menu-border");

    },
    function () {

        $(this).removeClass("sidebar-menu-border");
    })


})