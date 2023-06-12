
$(document).ready(function () {
    $('html, body').animate({
        scrollTop: '+90px'
    }, 1000);
});

function CheckHistoryLess(event, id) {

    event.preventDefault();
    this.blur();
    Swal.fire({
        title: 'Bạn có muốn chắc chắn đổi trạng thái ?',
        showCancelButton: true,
        confirmButtonText: 'OK',
        overlay: true,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Lesson/CheckHistoryLesson',
                type: 'POST',
                dataType: 'json',
                data: {
                    lessonId: id,
                },
                traditional: true,
                success: function (result) {
                    if (result == 1) {
                        $('#historyCheckbox_' + id).prop('checked', true); // Thay đổi giá trị checked thành true
                        if ($('#id_lesson').val() == id) {
                            $('#title-check-less').text('Đã học');
                        }
                    } else if (result == 0) {
                        $('#historyCheckbox_' + id).prop('checked', false);
                        if ($('#id_lesson').val() == id) {
                            $('#title-check-less').text('Chưa học');
                        }
                    } else {
                        MessageError("Lỗi ");
                    }
                },

                error: function () {
                    MessageError("Lỗi ");
                }
            });

        }
    })



}

function GetExamResult(courseid, employeeid, courseexamid) {
    employeeid = parseInt(employeeid);
    courseexamid = parseInt(courseexamid);

    var obj = { courseExamID: courseexamid, employeeID: employeeid };
    $.ajax({
        type: 'POST',
        url: '/Lesson/CreateExamResult',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(obj),
        success: function (response) {
            // After creating the exam result, load the answers
           
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
}





