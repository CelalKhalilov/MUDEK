using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        //[ForeignKey("StudentId")]
        //public int StudentId { get; set; }
        public string LowerRating { get; set; }
        public int Note { get; set; }
        //[InverseProperty("Questions")]
        //public Student Student { get; set; }

    }
}