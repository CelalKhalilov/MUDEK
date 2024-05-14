namespace Entities.Models
{
    public class AcademicUnit
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public List<Faculty>? FacultyList { get; set; }
        
    }

}