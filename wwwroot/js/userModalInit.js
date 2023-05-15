function editUser(userId)
{
    $.ajax({
        type: "GET",
        url: '/Users/Edit?id=' + userId,
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#user-edit-modal").modal("show");
           $('.modal-body').html(data);
        }
    })
}