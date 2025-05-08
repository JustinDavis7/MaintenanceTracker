$(document).ready(function () {
    $('#cancelEditButton').on('click', function (event) {
        event.preventDefault(); // Prevent default link behavior
        if (document.referrer) {
            window.location.href = document.referrer; // Navigate to the referrer page
        } else {
            window.location.href = '/PMTicket/Index'; // Fallback URL if no referrer
        }
    });

    $('form').on('submit', function (event) {
        event.preventDefault(); // Prevent default form submission
        var form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                if (document.referrer) {
                    window.location.href = document.referrer; // Navigate to the referrer page
                } else {
                    window.location.href = '/PMTicket/Index'; // Fallback URL if no referrer
                }
            },
            error: function () {
                alert('An error occurred while saving the PM Ticket.');
            }
        });
    });
});