namespace ManagementCourse.Models.DTO
{
    public class CourseDTO
    {
        public int ID { get; set; }
        public string NameCourse { get; set; }
        public int NumberLesson { get; set; } 
        public string Instructor { get; set; } 
        public bool Status { get; set; } 
    }
}
