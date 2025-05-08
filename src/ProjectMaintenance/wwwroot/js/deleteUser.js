function confirmDeleteUser(userName){
    let confirmation = confirm(`Do you want to delete ${userName}? Any tickets they have created will show your name on them as the creator now.`);
    if(confirmation)
    {
        deleteUser(userName);
        alert (`${userName}'s account has been deleted!`); 
    }
}

function deleteUser(userName){
    $.ajax({
        url: '/Admin/DeleteUser',
        type: 'POST',
        data: {userName: userName},
        success: function(result) {
            location.reload();
        },
        error: function(err) {
            console.error("Error deleting user: " + err)
        }
    });
}