$(document).ready(function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today createPM',
            center: 'title',
            right: 'viewPMIndex'
        },
        customButtons: {
            createPM: {
                text: 'Create PM',
                click: function() {
                    if (equipmentList && equipmentList.length > 0) {
                        $('#createPMModal').modal('show');
                    } else {
                        alert('No equipment available for PM ticket creation. Please add equipment before trying to create a PM ticket.');
                    }
                }
            },
            viewPMIndex: {
                text: 'View All PM\'s',
                click: function() {
                    $.ajax({
                        url: '/PMTicket/Index',
                        type: 'GET',
                        success: function(response) {
                            window.location.href = '/PMTicket/Index';
                        },
                        error: function() {
                            alert('Failed to view PMTickets.')
                        }
                    })
                }
            }
        },
        defaultDate: moment().format('YYYY-MM-DD'), // Set to today
        editable: false,
        events: function(start, end, timezone, callback) {
            $.ajax({
                url: '/PMTicket/GetPMTickets',
                dataType: 'json',
                success: function(data) {
                    var events = data.map(function(ticket) {
                        return {
                            title: ticket.equipmentName + ' - ' + ticket.title, // Combine title and equipment name
                            start: ticket.datePerformed, // Ensure this is in ISO8601 format
                            allDay: true, // Mark as all-day event
                            description: ticket.title, // You can use description for tooltips
                            id: ticket.ticketId // This is used to identify which ticket is here for the onclick function
                        };
                    });
                    callback(events);
                },
                error: function() {
                    alert('Failed to load events.');
                }
            });
        },
        eventRender: function(event, element) {
            var maxLength = 40; // Maximum characters to show

            // Truncate the description if it's longer than the max length
            var truncatedDescription = event.description.length > maxLength 
                ? event.description.substring(0, maxLength).split(" ").slice(0, -1).join(" ") + "..."
                : event.description;

            // Custom HTML structure for the event
            element.html(`
                <div class="event-card" data-event-id="${event.id}">
                    <div class="event-header">
                        ${event.title.split(' - ')[0]}
                        <span class="pm-due-label">- PM Due</span>
                    </div>
                    <div class="event-description">${truncatedDescription}</div>
                </div>
            `);
        
            // Click event handler
            element.on('click', function() {
                window.location.href = '/PMTicket/Details/' + event.id;
            });

            // Tooltip for the description
            element.qtip({
                content: event.description // Show the event description on hover
            });
        }
    });

    // Handle form submission for creating a new PMTicket
    $('#createPMForm').on('submit', function(event) {
        event.preventDefault();
        var formData = $(this).serialize(); // Serialize form data
        $.ajax({
            type: 'POST',
            url: '/PMTicket/Create', // Server-side endpoint
            data: formData,
            success: function(response) {
                if (response.success) {
                    // Close the modal
                    $('#createPMModal').modal('hide');

                    // Add the new event to the calendar
                    $('#calendar').fullCalendar('renderEvent', {
                        title: response.title,
                        start: response.date,
                        description: response.description
                    }, true); // Stick the event (persist it)

                    // Optionally, show a success message
                    alert('PM Ticket created successfully!');
                    window.location.reload();
                } else {
                    // Handle errors
                    alert('Failed to create PM Ticket: ' + response.message);
                }
            },
            error: function() {
                alert('An error occurred while creating the PM Ticket.');
            }
        });
    });
});
