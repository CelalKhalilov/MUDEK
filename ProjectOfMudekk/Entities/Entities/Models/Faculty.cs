namespace Entities.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string FacultyName { get; set; }
        public int AcademicUnitId { get; set; }
        // public AcademicUnit AcademicUnit { get; set; }
        public List<Department>? DepartmentList { get; set; }
    }
}