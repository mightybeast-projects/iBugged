function createTicket()
{
    $.ajax({
        type: "GET",
        url: '/Tickets/Create',
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#ticket-create-modal").modal("show");
            $('#ticket-create-modal .modal-body').html(data);
        }
    })
}

function editTicket(ticketId)
{
    $.ajax({
        type: "GET",
        url: '/Tickets/Edit?id=' + ticketId,
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#ticket-edit-modal").modal("show");
            $('#ticket-edit-modal .modal-body').html(data);
        }
    })
}