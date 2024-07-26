using FeedbackAPI.Models;
using FeedbackAPI.Dto;
using FluentValidation;
using System.Text.RegularExpressions;

namespace FeedbackAPI.Validators
{
    public class MessageValidator : AbstractValidator<MessageDto>
    {
        public MessageValidator() 
        { 
            RuleFor(message => message.TopicId).NotEmpty().GreaterThan(0);
            RuleFor(message => message.ContactId).NotEmpty().GreaterThan(0);
            RuleFor(message => message.MessageText).NotEmpty().MaximumLength(1024);
        }
    }

    public class MessagePostValidator : AbstractValidator<MessagePostDto>
    {
        public MessagePostValidator()
        {
            RuleFor(messagePost => messagePost.topicId).NotEmpty().GreaterThan(0).WithMessage("Wrong topic id");
            RuleFor(messagePost => messagePost.MessageText).NotEmpty().MaximumLength(1024).WithMessage("Wrong message. Should, it is empty");
            RuleFor(messagePost => messagePost.contactName).NotEmpty().MaximumLength(32).WithMessage("Wrong name");
            RuleFor(contact => contact.contactMail).EmailAddress().WithMessage("Wrong e-mail");
            RuleFor(contact => contact.contactPhone).NotEmpty().Length(10).Must(contact => Regex.IsMatch(contact!, @"^[0-9]{10}$")).WithMessage("Wrong phone number");
            RuleFor(messagePost => messagePost.captcha).NotEmpty().WithMessage("Empty captcha token. May you are bot??");
        }
    }
    
}
