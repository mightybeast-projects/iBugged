function createTicket()
{
    $.ajax({
        type: "GET",
        url: '/Tickets/Create',
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#ticket-create-modal").modal("show");
            $('.modal-body').html(data);
            applyMultiSelect();
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
           $('.modal-body').html(data);            
            applyMultiSelect();
        }
    })
}

function applyMultiSelect()
{
    $('.my-select').multiSelect(
    {
        selectableHeader: "<div class='custom-header'>Select members:</div>",
        selectionHeader: "<div class='custom-header'>Selected memebers:</div>"
    })
}