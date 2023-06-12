using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementCourse.Common
{
    public static class Config
    {
        /// <summary>
        /// Môi trường chạy
        /// 1: Môi trường Publish lên server
        /// 0: Môi trường Test trên local
        /// </summary>
        public static int _environment = 0;

       

        public static string Connection()
        {
            string conn = "";
            if (_environment == 0)
            {
               
                conn = @"server=DESKTOP-ABEN704\SQLEXPRESS;database=RTC;User Id = sa; Password=1;";
            }
            else
            {
                conn = "";
            }

            return conn;
        }

    }
}
