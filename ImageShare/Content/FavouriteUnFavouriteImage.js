$(document).ready(function () {

    $('.imageFavourites').hover(function () {
        $(this).css('cursor', 'pointer')
    },
    function () {
        $(this).css('cursor', '')
    });

    $('.imageFavourites').click(function () {
        var url = $(this).data('url');
        var change = $(this);

        $.get(url, function (data) {
            change.html(data);
        });
    });
});
