$(document).ready(function () {
    GetTaskType();
});
function GetTaskType() {

    $.ajax({
        url: '/Home/GetTaskType',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#dealType').append('<Option value=' + data.id + '>' + data.taskType + '</Option>')
            });
        }
    });
}

function TaskTypetext(sel) {
    $("#txtDealType").val((sel.options[sel.selectedIndex].text));
}
