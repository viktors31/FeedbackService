namespace FeedbackAPI.Dto
{
    public class MessagePostDto
    {
        public string contactName { get; set; }
        public string contactMail { get; set; }
        public string contactPhone { get; set; }
        public int topicId { get; set; }
        public string MessageText { get; set; }
    }
}
