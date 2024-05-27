using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.Entities.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }
        public string? ProfileImage { get; set; }
        public List<LearningOutcomes>? LearningOutcomes { get; set; }
        public List<Form>? Forms { get; set; }
        public List<AssessmentTool>? AssessmentTools { get; set; }
        public List<SubAssessmentTool>? SubAssessmentTools { get; set; }
        public List<Student>? Students { get; set; }
        public List<Message>? Messages { get; set; }

        public byte[]? Image { get; set; }
        public string? ProfileImageBase64 => Image != null ? Convert.ToBase64String(Image) : null;

    }
}
