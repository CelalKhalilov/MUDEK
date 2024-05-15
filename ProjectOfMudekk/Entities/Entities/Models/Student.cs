namespace Entities.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int StudentNumber { get; set; }
        public List<Question>? Questions { get; set; }
        public int TeacherId { get; set; }
    }
}