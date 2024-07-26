using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Mail {  get; set; }
        [MaxLength(10)]
        [MinLength(10)]
        public string Phone {  get; set; }
        [MaxLength(32)]
        public string Name {  get; set; } 
    }
}
