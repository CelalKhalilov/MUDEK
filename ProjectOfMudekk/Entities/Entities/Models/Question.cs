using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? LowerRating { get; set; }
        public int? Note { get; set; }
        public int StudentId { get; set; }

    }
}