namespace Entities.Models
{
    public class AcademicUnit
    {
        public int AcademicUnitId { get; set; }
        public string UnitName { get; set; }
        public int FacultyId { get; set; }
        public List<Faculty> FacultyList { get; set; }
        
    }

}