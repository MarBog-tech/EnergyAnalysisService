$(document).ready(function () {
    $('#searchDevices').click(function () {
        var searchTerm = $('#searchTerm').val();
        var energySourceCapacity = $('#energySourceCapacity').val();

        $.ajax({
            url: '/electricalload/searchdevices/")',
            type: "POST",
            data: {
                searchTerm: searchTerm,
                energySourceCapacity: energySourceCapacity
            },
            success: function (data) {
                $('#deviceSearchResults').html(data);
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + xhr.status + ' ' + xhr.statusText);
            }
        });
    });
});
    // $(document).on('click', '.add-device-btn', function () {
    //     var deviceId = $(this).data('device-id');
    //     var categoryId = $('#categoryId').val();
    //
    //     $.post('@Url.Action("AddDeviceToCategory", "Category")', { deviceId: deviceId, categoryId: categoryId }, function () {
    //         location.reload();
    //     });
    // });
