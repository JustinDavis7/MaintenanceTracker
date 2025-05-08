function showAddModal()
{
    $("#add-ticket-modal").modal("show");
}

function showArchivedModal()
{
    $("#archived-modal").modal("show");
}

// This is the function that runs when a ticket is being added after submit on the form is pressed.
$(document).ready(function() {
    $('#add-ticket-form').on('submit', function(e) {
        e.preventDefault();
        var title = document.getElementById("title").value;
        var equipmentElement = document.getElementById("equipmentId");
        var equipmentId = equipmentElement.value ? parseInt(equipmentElement.value) : null;
        var description = document.getElementById("description").value;
        var priorityLevel = document.getElementById("priorityLevel").value;
        var plannedCompletion = document.getElementById("plannedCompletion").value;
        var partsList = document.getElementById("partsList").value;
        var maintenanceType = document.getElementById("maintenanceType").value;
        var priorityBump = $('.switch input[type="checkbox"]').prop('checked')
        var requestCreationDate = document.getElementById("requestCreationDate").value;
        var ticketCreatorId = document.getElementById("ticketCreatorId").value;
        var worker = document.getElementById("assignedWorker").value;
        console.log("title=${title}&equipmentId=${equipmentId}&description=${description}&priorityLevel=${priorityLevel}&plannedCompletion=${plannedCompletion}&partsList=${partsList}&maintenanceType=${maintenanceType}&priorityBump=${priorityBump}&requestCreationDate=${requestCreationDate}&ticketCreatorId=${ticketCreatorId}&assignedWorker=${worker}");
        $.ajax({
            url: `MaintenanceTicket/Create?title=${title}&equipmentId=${equipmentId}&description=${description}&priorityLevel=${priorityLevel}&plannedCompletion=${plannedCompletion}&partsList=${partsList}
                    &maintenanceType=${maintenanceType}&priorityBump=${priorityBump}&requestCreationDate=${requestCreationDate}&ticketCreatorId=${ticketCreatorId}&assignedWorker=${worker}`,
            type: "POST",
            success: function(response) {
                $("#add-ticket-modal").modal("hide");
                window.location.reload();
            },
            error: function(response){
                console.log("The ticket wasn't added! - ", response);
                alert("The ticket wasn't added!");
            }
        });
    });
});

$(function() {
    // Read user roles from the hidden input field
    var userRoles = $('#userRoles').val().split(',');

    $(".kanban-column .scrollable-content").sortable({
        connectWith: ".scrollable-content",
        start: function(event, ui) {
            var ticketElement = ui.item;
            var currentStatus = ticketElement.closest('.kanban-column').find('h4').data('status').trim();

            // Check if sender is not null
            if (ui.sender) {
                // Your permission checks
            } else {
                console.log("UI sender is null, cannot cancel sorting.");
            }
        },
        receive: function(event, ui) {
            var ticketId = ui.item.data('ticket-id');
            var newStatus = $(this).parent().find('h4').data('status').trim();

            // Prevent maintenance users from reopening tickets
            if ((newStatus === "Active" && userRoles.includes("maintenance")) || // Prevent maintenance from reopening
                (newStatus === "Archived" && !(userRoles.includes("admin") || userRoles.includes("maintenanceLead"))) || // Prevent maintenance from archiving tickets
                (newStatus === "Open" && userRoles.includes("maintenance"))) 
                { // Prevent maintenance from reopening tickets
                // Revert to original position if not allowed
                $(ui.sender).sortable('cancel');
                alert("You do not have permission to move this ticket to the selected column.");
                console.log("Unauthorized drop attempt prevented.");
                return;
                }
        

            // Existing AJAX call for status update
            $.ajax({
                url: `/MaintenanceTicket/UpdateTicketStatus?ticketId=${ticketId}&newStatus=${newStatus}`,
                type: 'POST',
                success: function(response) {
                    if (newStatus === "Archived") {
                        ui.item.hide();
                        var ticketHtml = response.ticketHtml;
                        $("#archived-modal .scrollable-content").append(ticketHtml);
                    }
                },
                error: function(response){
                    $(ui.sender).sortable('cancel');
                    alert("Error updating ticket status: " + response.responseText);
                    console.log("Error updating ticket status: ", response.responseText);
                }
            });
        }
    }).disableSelection();
});

