using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ProgramAndLearningOutcomes
    {
        [Key]
        public int PId { get; set; }
        public int? P1 { get; set; }
        public int? P2 { get; set; }
        public int? P3 { get; set; }
        public int? P4 { get; set; }
        public int? P5 { get; set; }
        public int? P6 { get; set; }
        public int? P7 { get; set; }
        public int? P8 { get; set; }
        public int? P9 { get; set; }
        public int? P10 { get; set; }
        public int? P11 { get; set; }
        public int? P12 { get; set; }
    }
}