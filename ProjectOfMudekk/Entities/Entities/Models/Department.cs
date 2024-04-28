namespace Entities.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        
    }
}