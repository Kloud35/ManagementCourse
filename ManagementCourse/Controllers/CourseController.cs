using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ManagementCourse.Controllers
{
    public class CourseController : Controller
    {
        CourseRepository courseRepo ;

        public CourseController(CourseRepository courseRepository)
        {
            courseRepo = courseRepository;  
        }
        [HttpPost]
        public JsonResult GetListCourse(int id, string filterText)      
        {
            var listCourse = courseRepo.ListCourses(HttpContext.Session.GetInt32("department_id").Value, id, filterText);
      
            return Json(listCourse, new System.Text.Json.JsonSerializerOptions());
        }

    }
}
