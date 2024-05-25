using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}