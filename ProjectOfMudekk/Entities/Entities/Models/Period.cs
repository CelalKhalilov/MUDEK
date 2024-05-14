namespace Entities.Models
{
    public class Period
    {
        public int Id { get; set; }
        public string PeriodName { get; set; }
        public int DepartmentId { get; set; }
        public List<Lesson>? LessonList { get; set; }
    }
}