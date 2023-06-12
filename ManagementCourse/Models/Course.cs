﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ManagementCourse.Models
{
    public partial class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameCourse { get; set; }
        public string Instructor { get; set; }
        public int? CourseCatalogId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? DeleteFlag { get; set; }
        public int? FileCourseId { get; set; }
    }
}
