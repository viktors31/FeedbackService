using FeedbackAPI.Models;
using FeedbackAPI.Dto;
using FluentValidation;

namespace FeedbackAPI.Validators
{
    public class MessageTopicValidator : AbstractValidator<MessageTopicDto>
    {
        public MessageTopicValidator() 
        { 
            RuleFor(topic => topic.Name).NotEmpty().MaximumLength(32).WithMessage("Wrong name for topic");
        }
    }
}
