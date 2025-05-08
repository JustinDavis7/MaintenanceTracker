function openTicket(ticketId) {
    var requiredRoles = ["admin", "maintenanceLead"]; // Define roles allowed to open tickets
    if (hasRole(requiredRoles)) {
        $.ajax({
            url: `/MaintenanceTicket/UpdateTicketStatus?ticketId=${ticketId}&newStatus=active`,
            type: 'POST',
            success: function(response) {
                window.location.href = '/MaintenanceTicket/Index';
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.log("Error updating ticket status: ", textStatus, errorThrown);
                console.log("Response: ", jqXHR.responseText);
            }
        });
    } else {
        alert("You do not have permission to open this ticket.");
        console.log("Unauthorized action attempted to open the ticket.");
    }
}

function satisfyTicket(ticketId) {
    var requiredRoles = ["admin", "maintenance", "maintenanceLead"]; // Define roles allowed to satisfy tickets
    if (hasRole(requiredRoles)) {
        $.ajax({
            url: `/MaintenanceTicket/UpdateTicketStatus?ticketId=${ticketId}&newStatus=satisfied`,
            type: 'POST',
            success: function(response) {
                window.location.href = '/MaintenanceTicket/Index';
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.log("Error updating ticket status: ", textStatus, errorThrown);
                console.log("Response: ", jqXHR.responseText);
            }
        });
    } else {
        alert("You do not have permission to satisfy this ticket.");
        console.log("Unauthorized action attempted to satisfy the ticket.");
    }
}

function closeTicket(ticketId) {
    var requiredRoles = ["admin", "maintenance", "maintenanceLead"]; // Define roles allowed to close tickets
    if (hasRole(requiredRoles)) {
        $.ajax({
            url: `/MaintenanceTicket/UpdateTicketStatus?ticketId=${ticketId}&newStatus=closed`,
            type: 'POST',
            success: function(response) {
                window.location.href = '/MaintenanceTicket/Index';
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.log("Error updating ticket status: ", textStatus, errorThrown);
                console.log("Response: ", jqXHR.responseText);
            }
        });
    } else {
        alert("You do not have permission to close this ticket.");
        console.log("Unauthorized action attempted to close the ticket.");
    }
}

function archiveTicket(ticketId) {
    var requiredRoles = ["admin", "maintenanceLead"]; // Define roles allowed to archive tickets
    if (hasRole(requiredRoles)) {
        $.ajax({
            url: `/MaintenanceTicket/UpdateTicketStatus?ticketId=${ticketId}&newStatus=archived`,
            type: 'POST',
            success: function(response) {
                window.location.href = '/MaintenanceTicket/Index';
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.log("Error updating ticket status: ", textStatus, errorThrown);
                console.log("Response: ", jqXHR.responseText);
            }
        });
    } else {
        alert("You do not have permission to archive this ticket.");
        console.log("Unauthorized action attempted to archive the ticket.");
    }
}

// Helper function to check if the current user has the required roles
function hasRole(requiredRoles) {
    var userRoles = $('#userRoles').val().split(',');
    return requiredRoles.some(role => userRoles.includes(role));
}