function btnCreateProjectDetail() {

    if ($('#formprojectdetails').valid()) {

        var formData = new FormData();
        formData.append("ProjectTitle", $("#projectTitle").val());
        formData.append("ProjectPriority", $("#projectPriority").val());
        formData.append("ProjectDescription", $("#projectDescription").val());
        formData.append("ProjectStatus", $("#projectStatus").val());
        formData.append("ProjectDeadline", $("#projectdeadline").val());
        formData.append("ProjectType", $("#projectType").val());
        formData.append("ProjectHead", $("#projectHead").val());
        formData.append("ProjectLocation", $("#projectLocation").val());
        formData.append("ProjectStartDate", $("#projectStartDate").val());
        formData.append("ProjectEndDate", $("#projectEndDate").val());
        $.ajax({
            url: '/Project/CreateProject',
            type: 'Post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (Result) {

                Swal.fire({
                    title: Result.message,
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                }).then(function () {
                    window.location = '/Project/CreateProject';
                });     
            }
        })
    }
    else {
        Swal.fire({
            title: "Kindly Fill All Datafield",
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK',
        })
    }
}

$(document).ready(function () {

    $("#formprojectdetails").validate({
        rules: {
            projectPriority: "required",
            projectTitle: "required",
            projectDescription: "required",
            projectStatus: "required",
            projectdeadline: "required",
        },
        messages: {
            projectPriority: "Please Select Project Priority",
            projectTitle: "Please Enter Project Title",
            projectDescription: "Please Enter Project Description",
            projectStatus: "Please Enter Project Status",
            projectdeadline: "Please Enter Deadline Date",
        }
    });
    $("#frmprojectdetails").validate({
        rules: {
            projectType: "required",
            projectHead: "required",
            projectLocation: "required",
            projectStartDate: "required",
            projectEndDate: "required",
        },
        messages: {
            projectType: "Please Enter Project Type",
            projectHead: "Please Select Project Head",
            projectLocation: "Please Enter Location",
            projectStartDate: "Please Enter Start Date",
            projectEndDate: "Please Enter End Date",
        }
    });
    $('#projectDetails').on('click', function () {
        $('#frmprojectdetails').valid();
    });
});