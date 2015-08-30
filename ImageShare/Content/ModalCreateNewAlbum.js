$(document).ready(function () {
    $('#showDialog').click(function () {
        var url = $('#albumModal').data('url');

        $.get(url, function (data) {
            $('#modalContainer').html(data);

            $('#albumModal').modal('show');
        });
    });
});
