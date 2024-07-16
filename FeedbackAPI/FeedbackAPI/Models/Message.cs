using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Models
{
    public class Message
    {
        public int Id { get; set; } 
        public int TopicId {  get; set; }
        public int ContactId {  get; set; }
        [MaxLength(1024)]
        public string MessageText { get; set; }
        public DateTime PostDate {  get; set; }
    }
}
