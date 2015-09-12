$(document).ready(function () {
    $('.edit').click(function () {
        var url = $('#editCategory').data('url');
        var categoryid = $(this).data('url');

        url = url + categoryid;

        $.get(url, function (data) {
            $('#editModal').html(data);

            $('#editCategory').modal('show');
        });
    });
});
