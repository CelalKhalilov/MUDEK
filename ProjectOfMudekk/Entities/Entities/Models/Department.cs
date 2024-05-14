namespace Entities.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int FacultyId { get; set; }
        public List<Period>? PeriodList { get; set; }
        // public Faculty Faculty { get; set; }
        
    }
}