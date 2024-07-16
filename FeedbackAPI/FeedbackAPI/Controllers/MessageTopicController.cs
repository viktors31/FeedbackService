using AutoMapper;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageTopicController : ControllerBase
    {
        private readonly IUnitOfWork u;
        private readonly IMapper mapper;
        public MessageTopicController(IUnitOfWork u, IMapper mapper)
        {
            this.u = u;
            this.mapper = mapper;
        }
        //GET api/messagetopic - Get all topics
        [HttpGet]
        public async Task<IActionResult> GetMessageTopics()
        {
            var topics = await u.MessageTopicRepository.GetMessageTopics();
            //var topics_dto = mapper.Map<IEnumerable<MessageTopicDto>>(topics);
            return Ok(topics);
        }

        //GET api/messagetopic - Get topic
        /*[HttpGet("/{id}")]
        public async Task<IActionResult> GetMessageTopic(int id)
        {
            var topics = await u.MessageTopicRepository.GetMessageTopics();
            //var topics_dto = mapper.Map<IEnumerable<MessageTopicDto>>(topics);
            return Ok(topics);
        }*/

        //POST api/messagetopic - Post new topic in JSON Format
        [HttpPost]
        public async Task<IActionResult> AddMessageTopic(MessageTopic topic)
        {
            u.MessageTopicRepository.AddMessageTopic(topic);
            await u.SaveAsync();
            return Ok(topic);
        }

        //DELETE api/messagetopic - Delete topic by name
        [HttpDelete]
        public async Task<IActionResult> DeleteMessageTopic(MessageTopicDto topic)
        {
            u.MessageTopicRepository.DeleteMessageTopic(topic.Name);
            await u.SaveAsync();
            return Ok(topic);
        }

    }
}
