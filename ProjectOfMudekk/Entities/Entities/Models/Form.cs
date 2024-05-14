namespace Entities.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string AcademicUnit { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string EducationYear { get; set; }
        public string Period { get; set; }
        public string Lesson { get; set; }
        public string LessonCode { get; set; }
        public int TeacherId { get; set; }
    }
}