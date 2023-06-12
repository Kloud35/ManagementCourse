$(document).ready(function () {

    $('#value_search').keypress(function () {
        GetAllCourse($(this).val());
    });
    $("#value_search").on("input", function () {
        if ($(this).val().length == 0) {
            GetAllCourse($(this).val());
        }
    });
 
   

});
function CheckNullCourse(id, event) {
    event.preventDefault();
    
    $.ajax({
        url:'/Home/CheckNullCourse',
        type: 'POST',
        dataType: 'json',
        data: {
            id: id,
        },      
        success: function (result) {
            if (result == 0) {
                MessageWarning("Hiện tại chưa có bài học nào !");
            } else {
                window.location = `/Lesson/Index?courseId=${id}&lessionId=0`;
            }

        },

        error: function () {
            MessageError("Hiện tại chưa có bài học nào! ");
        }
    });


}
function GetAllCourse(text) {

    $.ajax({
        url:'/Course/GetListCourse',
        type: 'POST',
        dataType: 'json',
        data: {
            departmentId: 0,
            filterText: text
        },
       
        success: function (result) {
            let html = '';          
            $.each(result, function (key, item) {

               


                html += ` <a href="#" class="col-md-2 col-6 mt-2">
           
                <div class="card text-dark" style="height: 75px;" onclick=" return CheckNullCourse(${item.ID},event)">

                    <div class="card-body" style="padding: 0.5rem 0.5rem !important;">
                    <h5 class="card-title" style="font-size:15px ;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;">
                        ${item.NameCourse}
                            <p class="card-text" style="font-size:10px ;float:right;" >

                                <i class='bx bxs-book-alt text-info'></i> ${item.NumberLesson}
                            </p>

                        </h5>                        
                        <p class="card-text" style="font-size:10px">
                            ${item.Instructor}
                        </p>
                    </div>
                </div>
            
                 </a>`;
            });

            $('#grid_course').html(html);

        },

        error: function () {
            MessageError("Error loading data! Please try again.");
        }
    });


}
