namespace Entities.Models
{
    public class FileUpload
    {
        public int FileUploadId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
    }
}