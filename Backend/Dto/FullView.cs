using FeedbackAPI.Models;

namespace FeedbackAPI.Dto
{
    public class FullView
    {
        public Message message { get; set; }
        public Contact contact { get; set; }
        public MessageTopic messageTopic { get; set; }
    }
}
