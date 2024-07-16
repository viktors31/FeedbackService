using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Models
{
    public class MessageTopic
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name {  get; set; }
    }
}
