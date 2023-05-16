function createProject()
{
    $.ajax({
        type: "GET",
        url: '/Projects/Create',
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#project-create-modal").modal("show");
            $('.modal-body').html(data);
            applyMultiSelect();
        }
    })
}

function editProject(projectId)
{
    $.ajax({
        type: "GET",
        url: '/Projects/Edit?id=' + projectId,
        contentType: "application/text; charset=utf-8",
        success: function (data) {
            $("#project-edit-modal").modal("show");
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