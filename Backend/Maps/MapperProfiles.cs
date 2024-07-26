using AutoMapper;
using FeedbackAPI.Dto;
using FeedbackAPI.Models;

namespace FeedbackAPI.Maps
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            //Двухсторонний map для контакта
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();
            //Двухсторонний map для topic
            CreateMap<MessageTopic, MessageTopicDto>();
            CreateMap<MessageTopicDto, MessageTopic>();
            //Двухсторонний map для message
            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>();
        }
    }
}
