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
               
                conn = @"server=DESKTOP-40H717B\SQLEXPRESS;database=RTCTest;User Id = sa; Password=123456a@;";
            }
            else
            {
                conn = "";
            }

            return conn;
        }

    }
}
