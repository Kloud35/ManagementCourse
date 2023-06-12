using ManagementCourse.IReposiory;
using ManagementCourse.Models;
using ManagementCourse.Models.ViewModel;
using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagementCourse.Controllers
{
    public class CourseExamController : Controller
    {
        CourseExamRepository _courseExamRepo;
        IGenericRepository<CourseExamResultDetail> courseExamResultDetailRepo = new GenericRepository<CourseExamResultDetail>();
        IGenericRepository<CourseExamResult> courseExamResultRepo = new GenericRepository<CourseExamResult>();
        IGenericRepository<CourseRightAnswer> courseRightAnswerRepository = new GenericRepository<CourseRightAnswer>();
        IGenericRepository<CourseQuestion> courseQuestionRepository = new GenericRepository<CourseQuestion>();

        public CourseExamController(CourseExamRepository courseExamRepository)
        {
            _courseExamRepo = courseExamRepository;
        }
        public IActionResult Index(int courseId)
        {

            CourseExam courseExam = _courseExamRepo.GetCourseExam(courseId);

            var employeeID = HttpContext.Session.GetInt32("employeeid");
            var model = new CourseExamViewModel
            {
                CourseExam = courseExam,
                CourseQuestion = _courseExamRepo.GetCourseQuestion(courseExam.ID).OrderBy(p => p.Stt).ToList(),
                CourseAnswer = _courseExamRepo.GetCourseAnswer(),
                CourseExamResult = courseExamResultRepo.GetAll().OrderByDescending(p=>p.CreatedDate).FirstOrDefault(p => p.CourseExamId == courseExam.ID && p.EmployeeId == employeeID)
            };
            return View(model);
        }

        public JsonResult SaveQuestionAnswers([FromBody] List<CourseExamResultDetail> resultDetails)
        {
            // Lấy CourseExamResultId và CourseQuestionId từ resultDetails
            int courseExamResultId = resultDetails.FirstOrDefault()?.CourseExamResultId ?? 0;
            int courseQuestionId = resultDetails.FirstOrDefault()?.CourseQuestionId ?? 0;

            // Xóa các đáp án cũ trong cơ sở dữ liệu
            var existingResultDetails = courseExamResultDetailRepo.GetAll()
                .Where(p => p.CourseExamResultId == courseExamResultId && p.CourseQuestionId == courseQuestionId);
            courseExamResultDetailRepo.RemoveRange(existingResultDetails);

            // Thêm các đáp án mới
            foreach (var resultDetail in resultDetails)
            {
                courseExamResultDetailRepo.Create(resultDetail);
            }

            return Json(new { success = true });
        }

        public JsonResult GetExamResultDetail(int resultID)
        {
            return Json(courseExamResultDetailRepo.GetByID(resultID));
        }
        public IActionResult GetQuestionAnswers(int questionId, int courseExamResultId)
        {
            var previousResultDetails = courseExamResultDetailRepo.GetAll()
                .Where(p => p.CourseExamResultId == courseExamResultId && p.CourseQuestionId == questionId)
                .ToList();
            return Json(previousResultDetails);
        }

        public IActionResult GetQuestions(int courseExamId)
        {
            var questions = new CourseQuestionViewModel()
            {
                CourseQuestion = _courseExamRepo.GetCourseQuestion(courseExamId).OrderBy(p => p.Stt).ToList(),
                CourseAnswer = _courseExamRepo.GetCourseAnswer()
            };
            return Json(questions);
        }

        public IActionResult SubmitExam(int resultID)
        {
            var examResult = courseExamResultRepo.GetByID(resultID);
            var courseExam = _courseExamRepo.GetCourseExam((int)examResult.CourseExamId);

            var courseQuestions = courseQuestionRepository.GetAll().Where(p => p.CourseExamId == courseExam.ID).ToList();

            int numCorrectAnswers = 0;
            int numIncorrectAnswers = 0;

            foreach (var question in courseQuestions)
            {
                var courseRightAnswers = courseRightAnswerRepository.GetAll().Where(p => p.CourseQuestionId == question.Id);
                var examResultDetails = courseExamResultDetailRepo.GetAll().Where(p => p.CourseExamResultId == resultID && p.CourseQuestionId == question.Id);

                // Get the selected answer IDs from examResultDetails
                var selectedAnswerIDs = examResultDetails.Select(detail => detail.CourseAnswerId).ToList();

                // Check if the question has multiple correct answers
                bool hasMultipleCorrectAnswers = courseRightAnswers.Count() > 1;

                // Check if the selected answer IDs match all the correct answer IDs and there are no additional or missing answers
                var isCorrect = selectedAnswerIDs.Count() == courseRightAnswers.Count()
                    && selectedAnswerIDs.All(answerId => courseRightAnswers.Any(rightAnswer => rightAnswer.CourseAnswerId == answerId))
                    && courseRightAnswers.All(rightAnswer => selectedAnswerIDs.Contains(rightAnswer.CourseAnswerId));

                // If the question has multiple correct answers, consider it correct only if all correct answers are selected and there are no additional or missing answers
                // If the question has only one correct answer, consider it correct if the selected answer is the correct answer
                if ((hasMultipleCorrectAnswers && isCorrect) || (!hasMultipleCorrectAnswers && selectedAnswerIDs.Count == 1 && isCorrect))
                {
                    numCorrectAnswers++;
                }
                else
                {
                    numIncorrectAnswers++;
                }
            }




            examResult.TotalCorrect = numCorrectAnswers;
            examResult.TotalIncorrect = numIncorrectAnswers;
            examResult.PercentageCorrect = numCorrectAnswers != 0 ? ((decimal)numCorrectAnswers / (numCorrectAnswers + numIncorrectAnswers)) * 100 : (decimal?)null;

            courseExamResultRepo.Update(examResult);
            return Json(new { numCorrectAnswers, numIncorrectAnswers });
        }
        public IActionResult CheckExamCompletion(int resultID)
        {
            var examResult = courseExamResultRepo.GetByID(resultID);
            var totalCorrect = examResult.TotalCorrect;
            var totalIncorrect = examResult.TotalIncorrect;
            if (examResult != null)
            {
                bool isCompleted = examResult.PercentageCorrect != null;
                return Json(new { isCompleted, totalCorrect, totalIncorrect });
            }

            return Json(new { isCompleted = false });
        }
        public IActionResult RetakeExam(int resultID)
        {
           
            // Delete existing CourseExamResult record
            var existingResult = courseExamResultRepo.GetByID(resultID);
            var courseExamID = existingResult.CourseExamId;
            var employeeID = HttpContext.Session.GetInt32("employeeid");
            var fullname = HttpContext.Session.GetString("fullname");
            // Create a new CourseExamResult for the user to retake the exam
            var newResult = new CourseExamResult()
            {
                CourseExamId = courseExamID,
                EmployeeId = employeeID,
                TotalCorrect = 0,
                TotalIncorrect = 0,
                CreatedBy = fullname,
                CreatedDate = DateTime.Now,
                UpdatedBy = fullname,
                UpdatedDate = DateTime.Now
            };
            courseExamResultRepo.Create(newResult);

            return RedirectToAction("Index", "CourseExam", new { courseId = courseExamID });

        }

    }
}
