using FeedbackAPI.Models;
using FeedbackAPI.Dto;
using FluentValidation;
using System.Text.RegularExpressions;

namespace FeedbackAPI.Validators
{
    public class ContactValidator : AbstractValidator<ContactDto>
    {
        public ContactValidator() 
        {
            RuleFor(contact => contact.Name).NotEmpty().MaximumLength(32).WithMessage("Wrong name");
            RuleFor(contact => contact.Mail).EmailAddress().WithMessage("Wrong e-mail");
            RuleFor(contact => contact.Phone).NotEmpty().Length(10).Must(contact => Regex.IsMatch(contact!, @"^[0-9]{10}$")).WithMessage("Wrong phone number");
        }
    }
}
