var colorMap = new Map();

colorMap.set("Low", '#00c900');
colorMap.set("Medium", '#e3cc00');
colorMap.set("High", '#ff0000');
colorMap.set("Bug", '#ff0000');
colorMap.set("Task", '#9a00c9');
colorMap.set("Support", '#00c3c9');
colorMap.set("Other", '#969696');
colorMap.set("Closed", '#00c900');
colorMap.set("Opened", '#ff0000');

function drawTags() {
    $(".ticket-tag").each(function() {
        $(this).css("background-color", colorMap.get($(this).text()));
    })
}