using AutoMapper;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageTopicController : ControllerBase
    {
        private readonly IUnitOfWork u;
        private readonly IMapper mapper;
        private readonly IValidator<MessageTopicDto> validator;
        public MessageTopicController(IUnitOfWork u, IMapper mapper, IValidator<MessageTopicDto> validator)
        {
            this.u = u;
            this.mapper = mapper;
            this.validator = validator;
        }
        //Функция валидатор ID
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool IdValid(int id)
        {
            return (id < 1) ? false : true;
        }
        //GET api/messagetopic - Get all topics
        [HttpGet]
        public async Task<IActionResult> GetMessageTopics()
        {
            var topics = await u.MessageTopicRepository.GetMessageTopics();
            return Ok(topics);
        }

        //GET api/messagetopic - Get topic
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageTopic(int id)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var topic = u.MessageTopicRepository.GetMessageTopicById(id);
            if (topic == null)
                return BadRequest("Topic with current ID does not exist");
            return Ok(topic);
        }

        //POST api/messagetopic - Post new topic in JSON Format
        [HttpPost]
        public async Task<IActionResult> AddMessageTopic(MessageTopicDto topic)
        {
            var validateResult = validator.Validate(topic);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new{ err.AttemptedValue, err.ErrorMessage}) });
            var newTopic = mapper.Map<MessageTopic>(topic);
            u.MessageTopicRepository.AddMessageTopic(newTopic);
            await u.SaveAsync();
            return Ok(newTopic);
        }

        //PATCH api/messagetopic - Patch existing topic
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMessageTopic(int id, MessageTopicDto patchMessageTopic)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            /*var validateResult = validator.Validate(patchMessageTopic);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });*/
            var topicFromDb = u.MessageTopicRepository.GetMessageTopicById(id);
            if (topicFromDb == null)
                return BadRequest("Topic with current ID does not exist");
            mapper.Map(patchMessageTopic, topicFromDb);
            await u.SaveAsync();

            return NoContent();

        }

        //PATCH api/messagetopic/json - Patch existing topic by JSON Parser
        [HttpPatch("json/{id}")]
        public async Task<IActionResult> PatchMessageTopicJSON(int id, [FromBody] JsonPatchDocument<MessageTopicDto> patchMessageTopic)
        {
            var topicFromDb = u.MessageTopicRepository.GetMessageTopicById(id); //достать топик из БД
            if (topicFromDb == null)
                return BadRequest("Topic with current ID does not exist");
            var topicFromDbDTO = mapper.Map<MessageTopicDto>(topicFromDb); //Привести запись из бд к виду DTO
            patchMessageTopic.ApplyTo(topicFromDbDTO); //Перекинуть JSON в запись из бд в формате DTO
            //Это важно, поскольку в теле представлен топик DTO, ApplyTo можно применить только к топику DTO
            //Нельзя переписать ApplyTo DTO Topic из запроса в БД топик, так как типы разные!
            mapper.Map(topicFromDbDTO, topicFromDb); //Смаппить DTO формат обратно в обычный
            await u.SaveAsync(); //Сохраняем обновленную сущность

            return NoContent();

        }

        //PUT api/messagetopic - Put existing topic
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageTopic(int id, MessageTopicDto putMessageTopic)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var validateResult = validator.Validate(putMessageTopic);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            var topicFromDb = u.MessageTopicRepository.GetMessageTopicById(id); //достать из БД
            if (topicFromDb == null)
                return BadRequest("Topic with current ID does not exist");
            mapper.Map(putMessageTopic, topicFromDb); //смапить
            await u.SaveAsync(); //сохранить

            return NoContent();

        }

        //DELETE api/messagetopic - Delete topic by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageTopic(int id)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var selectedTopic = u.MessageTopicRepository.GetMessageTopicById(id);
            if (selectedTopic != null)
            {
                u.MessageTopicRepository.DeleteMessageTopic(selectedTopic);
                await u.SaveAsync();
            }
            else
                return BadRequest("Topic with current ID does not exist");
            return Ok(selectedTopic);
        }


    }
}
