using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Dto
{
    public class ContactDto
    {
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
    }
}
