using ManagementCourse.Common;
using ManagementCourse.Models;
using ManagementCourse.Models.DTO;
using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementCourse.Controllers
{
    public class CourseExamResultController : Controller
    {
        CourseExamResultRepository examResultRepo = new CourseExamResultRepository();
        CourseExamResultDetailRepository resultDetailRepo = new CourseExamResultDetailRepository();
        CourseRepository courseRepo = new CourseRepository();
        CourseQuestionRepository questionRepo = new CourseQuestionRepository();

        public IActionResult Index(int courseId)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Course course = courseRepo.GetByID(courseId);
            CourseExam courseExam = SQLHelper<CourseExam>.SqlToModel($"SELECT TOP 1 * FROM dbo.CourseExam WHERE CourseId = {courseId}");

            ViewBag.CourseId = courseId;
            ViewBag.CourseName = course == null ? "" : course.NameCourse;
            ViewBag.CourseExamID = courseExam.ID;

            return View();
        }

        public JsonResult GetExamResult(int courseId)
        {
            int employeeId = (int)HttpContext.Session.GetInt32("employeeid");
            List<CourseExamResultDTO> listExamResult = SQLHelper<CourseExamResultDTO>.ProcedureToList("spGetCourseExamResult",
                                                                                            new string[] { "@CourseExamResultID", "@CourseID", "@EmployeeID" },
                                                                                            new object[] { 0, courseId, employeeId });
            return Json(listExamResult, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        public JsonResult CreateExamResult([FromBody] CourseExamResult examResult)
        {
            try
            {
                CourseExamResult courseExamResult = new CourseExamResult();

                courseExamResult.CourseExamId = examResult.CourseExamId;

                courseExamResult.EmployeeId = HttpContext.Session.GetInt32("employeeid");
                courseExamResult.TotalCorrect = courseExamResult.TotalIncorrect = 0;
                courseExamResult.PercentageCorrect = 0;

                courseExamResult.CreatedBy = courseExamResult.UpdatedBy = HttpContext.Session.GetString("loginname");
                courseExamResult.CreatedDate = courseExamResult.UpdatedDate = DateTime.Now;

                int id = 0;

                if (examResultRepo.Create(courseExamResult) == 1)
                {
                    id = courseExamResult.Id;
                    return Json(id, new System.Text.Json.JsonSerializerOptions());
                }
                else
                {
                    return Json(id, new System.Text.Json.JsonSerializerOptions());
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public JsonResult GetExamQuestion(int courseId, int courseExamResultID)
        {
            var list = questionRepo.ListExamQuestion(courseId, courseExamResultID);
            return Json(list, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        public JsonResult CreateExamResultDetail([FromBody] List<CourseExamResultDetail> details)
        {
            int questionId = 0;
            List<int> listAnswerId = new List<int>();

            try
            {
                if (details.Count <= 0)
                {
                    return Json(1, new System.Text.Json.JsonSerializerOptions());
                }
                var existingResultDetails = resultDetailRepo.GetAll().Where(p => p.CourseExamResultId == details.First().CourseExamResultId && p.CourseQuestionId == details.First().CourseQuestionId).ToList();

                resultDetailRepo.RemoveRange(existingResultDetails);

                foreach (CourseExamResultDetail item in details)
                {
                    CourseExamResultDetail detail = item;

                    detail.CreatedDate = item.UpdatedDate = DateTime.Now;
                    detail.CreatedBy = item.UpdatedBy = HttpContext.Session.GetString("loginname");

                    if (resultDetailRepo.Create(detail) == 1)
                    {
                        questionId = (int)detail.CourseQuestionId;
                        listAnswerId.Add((int)detail.CourseAnswerId);
                    }
                }

                var tuple = new Tuple<int, List<int>>(questionId, listAnswerId);

                return Json(tuple, new System.Text.Json.JsonSerializerOptions());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
