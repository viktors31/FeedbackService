using AutoMapper;
using FeedbackAPI.Data;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork u;
        private readonly IMapper mapper;
        public MessageController(IUnitOfWork u, IMapper mapper)
        {
            this.u = u;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var message_list = await u.MessageRepository.GetMessages();
            return Ok(message_list);
        }
        [HttpGet("{id}")]
        public async Task <IActionResult> GetMessageById(int id)
        {
            var full_message = await u.MessageRepository.GetMessageById(id);
            return Ok(full_message);
        }
        [HttpPost]
        public async Task<IActionResult> AddMessage(MessagePostDto message)
        {
            //Поиск и проверка что номер топика реальный
            var selectedTopic = u.MessageTopicRepository.GetMessageTopicById(message.topicId);

            //ищем контакт в бд, если нет то добавим новый
            Contact contact = u.ContactRepository.GetContactByData(message.contactMail, message.contactPhone);
            if (contact == null)
            {
                contact = new Contact
                {
                    Name = message.contactName,
                    Phone = message.contactPhone,
                    Mail = message.contactMail,
                };
                u.ContactRepository.AddContact(contact);
                u.Save();
                contact = u.ContactRepository.GetContactByData(message.contactMail, message.contactPhone);
            }
            var newMessage = new Message
            {
                TopicId = message.topicId,
                ContactId = contact.Id,
                MessageText = message.MessageText,
                PostDate = DateTime.Now
            };
            u.MessageRepository.AddMessage(newMessage);
            await u.SaveAsync();
            var db_record = new FullView
            {
                messagetopic = selectedTopic,
                message = newMessage,
                contact = contact,
            };
            return Ok(db_record);
        }
    }
}
