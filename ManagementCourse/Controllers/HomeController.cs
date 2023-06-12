
using ManagementCourse.Common;
using ManagementCourse.Models;
using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace ManagementCourse.Controllers
{
    public class HomeController : Controller
    {
        
        DepartmentRepository _departmentRepo;   
        CourseCatalogRepository _courseCatalogRepo ;
        CourseRepository _courseRepo ;
        LessonRepository _lessonRepo ;
   
        public HomeController(DepartmentRepository departmentRepository, CourseCatalogRepository courseCatalogRepository, CourseRepository courseRepository, LessonRepository lessonRepository)
        {
           
            _departmentRepo = departmentRepository;
            _courseCatalogRepo = courseCatalogRepository;
            _courseRepo = courseRepository;
            _lessonRepo = lessonRepository ;
        }
    
       
      
        public IActionResult Index(int id)
        {
            var idDepartment = HttpContext.Session.GetInt32("department_id") ?? 0;
            var listCourse = _courseRepo.ListCourses(idDepartment, id,null);
            if (listCourse.Count < 1)
                ViewBag.TitleCourse = "Hiện tại chưa có khóa học nào !";
            else if (id == 0)
                ViewBag.TitleCourse = "Tất cả khóa học của phòng  " + _departmentRepo.GetByID(idDepartment).Name;
            else
                ViewBag.TitleCourse = _courseCatalogRepo.GetByID(id).Name;
            return View(listCourse);
        }
        #region Login    
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập username và mật khẩu !";
                return View();
            }
            password = string.IsNullOrEmpty(password) ? "" : Common.MaHoaMD5.EncryptPassword(password);
            DataTable user = LoadDataFromSP.GetDataTableSP("spLogin",
                                            new string[] { "@LoginName", "@Password" }, new object[] { username, password });
            if (user.Rows.Count > 0)
            {
                //int isAdmin = Convert.ToInt32(TextUtils.ToInt(user.Rows[0]["IsAdmin"]));
                HttpContext.Session.SetInt32("userid", TextUtils.ToInt(user.Rows[0]["ID"]));
                HttpContext.Session.SetInt32("employeeid", TextUtils.ToInt(user.Rows[0]["EmployeeID"]));
                //HttpContext.Session.SetString("login_name", TextUtils.ToString(user.Rows[0]["LoginName"]));
                HttpContext.Session.SetString("fullname", TextUtils.ToString(user.Rows[0]["FullName"]));
                //HttpContext.Session.SetInt32("isAdmin", isAdmin);
                //HttpContext.Session.SetInt32("isAdminSale", TextUtils.ToInt(user.Rows[0]["IsAdminSale"]));
                HttpContext.Session.SetInt32("department_id", TextUtils.ToInt(user.Rows[0]["DepartmentID"]));
                HttpContext.Session.SetString("department", TextUtils.ToString(user.Rows[0]["DepartmentName"]));
                //HttpContext.Session.SetInt32("headOfDepartment", TextUtils.ToInt(user.Rows[0]["HeadofDepartment"]));
                //HttpContext.Session.SetInt32("role_id", TextUtils.ToInt(user.Rows[0]["RoleID"]));
                //HttpContext.Session.SetString("role", TextUtils.ToString(user.Rows[0]["RoleName"]));
                //HttpContext.Session.SetString("img", TextUtils.ToString(user.Rows[0]["AnhCBNV"]));
                //HttpContext.Session.SetInt32("userteamid", TextUtils.ToInt(user.Rows[0]["UserTeamID"]));
                //HttpContext.Session.SetInt32("isleader", TextUtils.ToInt(user.Rows[0]["IsLeader"]));
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu !";
            }

            return View(user);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
           
            return RedirectToAction("Login");
        }
        #endregion
        [HttpPost]
        public IActionResult CheckNullCourse(int id)
        {
            var lstLess =  _lessonRepo.GetAllList().Where(c => c.CourseId == id);
            if (lstLess.Count() < 1)
                return Json(0, new System.Text.Json.JsonSerializerOptions());
            return Json(1, new System.Text.Json.JsonSerializerOptions());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

    }
}
