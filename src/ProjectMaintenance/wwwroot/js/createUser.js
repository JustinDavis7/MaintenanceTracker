function validateForm() {
    const nameInput = document.getElementById("name").value;
    const regex = /^[a-zA-Z0-9\s]*$/;

    if (!regex.test(nameInput)) {
        alert("Username must contain only alphanumeric characters.");
        return false;
    }
    return true;
}

$(document).ready(function () {
    $("#create-user-form").submit(function (e) {
        e.preventDefault();

        if (!validateForm()) {
            return; // If validation fails, prevent the form from being submitted
        }

        var form = $(this);
        var url = form.attr('action');
        var formData = form.serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {
                if (response.success) {
                    alert("User created successfully!");
                    // Optionally, you could redirect or clear the form here
                    // window.location.href = '/Admin/ShowAllUsers'; // Redirect
                    // form.trigger("reset"); // Clear form
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("An unexpected error occurred: " + error);
            }
        });
    });
});
