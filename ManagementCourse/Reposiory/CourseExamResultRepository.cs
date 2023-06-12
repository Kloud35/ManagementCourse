using ManagementCourse.Models;
using ManagementCourse.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManagementCourse.Reposiory
{
    public class CourseExamResultRepository
    {
        RTCContext _context = new RTCContext();
        public CourseExamResultRepository()
        {
        }
        public int Create(CourseExamResult examResult)
        {
            CourseExamResult result = new CourseExamResult()
            {
                CourseExamId = examResult.CourseExamId,
                EmployeeId = examResult.EmployeeId,
                TotalCorrect = 0,
                TotalIncorrect = 0,
                PercentageCorrect = 0,
                CreatedBy = "",
                CreatedDate = DateTime.Now,
                UpdatedBy = "",
                UpdatedDate = DateTime.Now

            };
            _context.CourseExamResults.Add(result);

            return _context.SaveChanges();

        }
    }
}
