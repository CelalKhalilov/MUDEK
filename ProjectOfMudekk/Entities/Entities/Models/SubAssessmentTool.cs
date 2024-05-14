namespace Entities.Models
{
    public class SubAssessmentTool
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LowerRating { get; set; }
        public int Point { get; set; }
        public int TeacherId { get; set; }
    }
}