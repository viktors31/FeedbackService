using AutoMapper;
using FeedbackAPI.Dto;
using FeedbackAPI.Models;

namespace FeedbackAPI.Maps
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            //Представлем клиенту модели без ID
            CreateMap<Contact, ContactDto>();
            CreateMap<MessageTopic, MessageTopicDto>();
            //Map для удаления топика по названию
            CreateMap<MessageTopicDto, MessageTopic>();
        }
    }
}
