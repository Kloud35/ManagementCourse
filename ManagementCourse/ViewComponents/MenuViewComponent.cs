using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ManagementCourse.Component
{
    public class MenuViewComponent:ViewComponent
    {
        DepartmentRepository departmentRepo = new DepartmentRepository();
        CourseCatalogRepository courseCatalogRepo = new CourseCatalogRepository();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var idDepartment = HttpContext.Session.GetInt32("department_id") ?? 0;
            ViewBag.ListDepartment =  departmentRepo.GetAllList().Where(c=>c.Id == idDepartment);
            ViewBag.ListcourseCatalog = courseCatalogRepo.GetAllList().Where(c=>c.DeleteFlag == true);
            return View();


        }
    }
}
