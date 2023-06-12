﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ManagementCourse.Models
{
    public partial class CourseExam
    {
        public int ID { get; set; }
        public string NameExam { get; set; }
        public string CodeExam { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CourseId { get; set; }
    }
}
