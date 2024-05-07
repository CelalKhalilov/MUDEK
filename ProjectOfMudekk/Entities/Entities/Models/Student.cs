namespace Entities.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public int StudentNumber { get; set; }
        public List<Question> Questions { get; set; }
    }
}