using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Dto
{
    public class MessageDto
    {
        public int TopicId { get; set; }
        public int ContactId { get; set; }
        [MaxLength(1024)]
        public string MessageText { get; set; }
    }

    public class MessagePostDto
    {
        public string contactName { get; set; }
        public string contactMail { get; set; }
        public string contactPhone { get; set; }
        public int topicId { get; set; }
        public string MessageText { get; set; }
        //Не забываем про капчу токен!!!
        public string captcha { get; set; }
    }
}
