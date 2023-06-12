using ManagementCourse.Common;
using ManagementCourse.Models;
using System.Collections.Generic;
using System.Data;
using ManagementCourse.Models.DTO;
using System.Linq;

namespace ManagementCourse.Reposiory
{
    public class CourseRepository:GenericRepository<Course>
    {
        public List<CourseDTO> ListCourses(int departmentID,int courseCatalogID, string filterText)
        {
            List<CourseDTO> list = new List<CourseDTO>();
            DataSet dataSet = LoadDataFromSP.GetDataSetSP("spGetCourse",
                                                            new string[] { "@DepartmentID", "@CourseCatalogID", "@FilterText" },
                                                            new object[] { departmentID, courseCatalogID, filterText });
            DataTable dt = dataSet.Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = TextUtils.ConvertDataTable<CourseDTO>(dt);

            }

            return list.Where(c=>c.Status == true).ToList();
        }
    }
}
