function showAddModal()
{
    $("#add-equipment-modal").modal("show");
}

$(document).ready(function() {
    $('#add-equipment-form').on('submit', function(e) {
        e.preventDefault();
        var formData = $(this).serialize();
        $.ajax({
            url: `Equipment/Create`,
            type: "POST",
            data: formData,
            success: function(response) {
                $("#add-equipment-modal").modal("hide");
                window.location.reload();
            },
            error: function(response){
                console.log("The equipment wasn't added! - ", response);
            }
        });
    });
});