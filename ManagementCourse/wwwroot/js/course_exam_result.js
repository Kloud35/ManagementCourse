var examquestions = [];
var courseExamResultID = 0;
var indexQuestion = 0;

$(document).ready(function () {
    GetExamResult();
})

//Get danh sách kết quả
function GetExamResult() {
    $.ajax({
        url: "/CourseExamResult/GetExamResult",
        type: "GET",
        dataType: 'json',
        contentType: 'application/json',
        data: { courseId: courseId },
        success: function (response) {

            //console.log(response);

            var html = '';
            $.each(response, function (i, item) {
                html += `<tr>
                            <td class="p-1">${item.NameCourse}</td>
                            <td class="p-1">${item.NameExam}</td>
                            <td class="text-end p-1">${item.TotalQuestion}</td>
                            <td class="text-end p-1">${item.TotalCorrect}</td>
                            <td class="text-end p-1">${item.TotalIncorrect}</td>
                            <td class="text-end p-1">${item.PercentageCorrect}</td>
                            <td class="text-center p-1">${moment(item.CreatedDate).format("DD/MM/YYYY HH:mm")}</td>
                        </tr>`;
            })

            $('#tbody_exam_result').html(html);
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}


//Sự kiện khi click làm bài thi
function onStart(isSuccess, examResultId) {

    var obj = {
        CourseExamId: parseInt(courseExamId)
    }

    if (!isSuccess) {
        $.ajax({
            url: "/CourseExamResult/CreateExamResult",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            success: function (response) {

                if (parseInt(response) > 0) {
                    courseExamResultID = parseInt(response);

                    $('#modal_exam_test').modal('show');
                    GetExamQuestion(courseId, parseInt(response));

                } else {
                    alert("Xảy ra lỗi khi tạo bài thi!");
                }

            },
            error: function (error) {
                alert(error);
            }
        });
    } else {
        $('#modal_exam_test').modal('show');
        GetExamQuestion(courseId, examResultId);
    }
}

//Get danh sách câu hỏi
function GetExamQuestion(courseId, courseExamResultID) {
    $.ajax({
        url: "/CourseExamResult/GetExamQuestion",
        type: "GET",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            courseId: courseId,
            courseExamResultID: courseExamResultID
        },
        success: function (response) {
            examquestions = response;

            addNumberQuestion(examquestions);
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}

//Sự kiện khi click next
function onNext() {
    AddResultDetail();

    $('.current-question').removeClass('current-question');

    if (examquestions.length > 0) {
        
        var item = examquestions[indexQuestion];

        addContentQuestion(item);

        $(`span[id="${item.ID}"]`).addClass('current-question');

        if (indexQuestion < examquestions.length - 1) {
            indexQuestion++;
        }
    }
}

//sự kiện khi click prev
function onPrevios() {
    $('.current-question').removeClass('current-question');

    if (examquestions.length > 0) {
        

        var item = examquestions[indexQuestion];

        addContentQuestion(item);
        $(`span[id="${item.ID}"]`).addClass('current-question');

        if (indexQuestion > 0) {
            indexQuestion--;
        }
    }
}

//Sự kiện khi click chọn câu hỏi
function onChosenQuestion(event, id) {
    $('.current-question').removeClass('current-question');

    var currentAnswer = $(event.target).attr('id');
    
    var item = examquestions.find(x => x.ID == id);

    indexQuestion = examquestions.findIndex(x => x.ID == id);

    if (currentAnswer == item.ID) {
        $(event.target).addClass('current-question');
    }

    addContentQuestion(item)
}

//Get đáp án
function AddResultDetail() {

    var answers = [];

    $('.form-check-input').each(function () {

        var checked = $(this).is(':checked');
        if (checked) {

            var obj = {
                AnswerText: $(`label[for="${$(this).attr('id')}"]`).html(),
                CourseQuestionId: parseInt($(this).attr('name')),
                CourseAnswerId: parseInt($(this).attr('id')),
                CourseExamResultId: courseExamResultID
            }

            answers.push(obj);

        }
        
    })

    //console.log(answers);

    $.ajax({
        url: "/CourseExamResult/CreateExamResultDetail",
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(answers),
        success: function (response) {
            console.log('insert detail:',response);

            //Update arr examquestions
            var item = examquestions.find(x => x.ID == response.Item1);

            if (item != null) {
                item.QuestionChosenID = response.Item1;

                $.each(response.Item2, function (key, data) {
                    var answer = item.ExamAnswers.find(x => x.ID == data);
                    answer.CourseAnswerChosenID = data;
                })

                addNumberQuestion(examquestions);
            }

            
        },
        error: function (error) {
            alert(error);
        }
    });

}

//Show câu hỏi lên giao diện
function addContentQuestion(item) {

    var inputname = item.ID;

    var htmlContent = `<input type="hidden" id="course_exam_question_id_${item.ID}" value="${item.ID}" />
                        <p class="text-dark fs-5 fw-bold">
                            ${item.QuestionText}
                        </p>`;

    var htmlAnswer = '';
    $.each(item.ExamAnswers, function (i, item) {
        var checked = item.ID == item.CourseAnswerChosenID ? 'checked' : '';
        htmlAnswer += `<div class="form-check form-check-exam-answer">
                                    <input class="form-check-input rounded-0" type="checkbox" name="${inputname}" id="${item.ID}" ${checked}>
                                    <label class="form-check-label text-dark" for="${item.ID}">
                                        ${item.AnswerText}
                                    </label>
                                </div>`;

    })

    var html = htmlContent + htmlAnswer
    $('#exam_question_content').html(html);
}

//Add number question
function addNumberQuestion(data) {
    var html = '';
    $.each(data, function (key, item) {

        var classSuccess = item.QuestionChosenID == item.ID ? 'question-success' : '';

        html += `<span class="question-number m-1 border border-dark ${classSuccess}" id="${item.ID}"
                            onclick="return onChosenQuestion(event,${item.ID});">${key + 1}</span>`;
    })

    $('#exam_question_navigation').html(html);

    $(".question-number").css({
        display: "inline-block",
        width: "30px",
        height: "30px",
        textAlign: "center",
        lineHeight: "30px",
        border: "1px solid #000",
        cursor: "pointer"
    });
}