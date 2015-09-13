$(document).ready(function () {

    $('.showText').click(function () {

        url = $(this).find('.showImageModal').data('url');
        var object = $(this);

        $.get(url, function (data) {

            $('#showImageModal').html(data);
            $('#showImageModal').modal('show');
        });
    });
});