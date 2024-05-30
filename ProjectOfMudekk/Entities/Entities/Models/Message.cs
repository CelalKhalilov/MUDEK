namespace Entities.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Chat { get; set; }
        public DateTime SentDate { get; set; }
        public int TeacherId { get; set; }
        public int SenderId { get; set; }
        public bool IsNew { get; set; }  // Yeni alan
    }
}