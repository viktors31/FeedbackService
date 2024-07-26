using AutoMapper;
using Azure.Core.Serialization;
using FeedbackAPI.Data;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System;
using System.Reflection;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork u;
        private readonly IMapper mapper;
        private readonly IValidator<MessageDto> validator;
        private readonly IValidator<MessagePostDto> validatorPost;
        private readonly IHttpClientFactory HttpClient;
        public MessageController(IUnitOfWork u, IMapper mapper, IValidator<MessageDto> validator, IValidator<MessagePostDto> validatorPost, IHttpClientFactory httpClient)
        {
            this.u = u;
            this.mapper = mapper;
            this.validator = validator;
            this.validatorPost = validatorPost;
            this.HttpClient = httpClient;
            //httpClient.CreateClient(); //создаю клиент в конструкторе
        }
        //Функция валидатор ID
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool IdValid(int id)
        {
            return (id < 1) ? false : true;
        }
        //Отправка запроса на проверку капчи
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckCaptcha(string captcha)
        {
            //Для отправки используем x-www-form-urlencoded, словарь в помощь
            var data = new Dictionary<string, string>();
            data.Add("secret", "6Le_wBQqAAAAAF-VWWZ8j_NZR7_PAYUHeGw1iyRZ");
            data.Add("response", captcha);
            //Создание нового запросика с ссылкой, заголовком и контентом
            var captchaGoogleCheck = new HttpRequestMessage(
            HttpMethod.Post,
            "https://www.google.com/recaptcha/api/siteverify")
            {
                Headers =
            {
                { HeaderNames.UserAgent, "ASP.NET Core Application" }
            },
                Content = new FormUrlEncodedContent(data)
            };
            var HttpClientForGoogle = HttpClient.CreateClient(); //Создать клиента
            var captchaResponse = HttpClientForGoogle.Send(captchaGoogleCheck); //Отправить и получить ответ
            if (captchaResponse.IsSuccessStatusCode) //Если запрос успешно прошел
            {
                /*var stream = captchaResponse.Content.ReadAsStream();
                var data2 = JsonSerializer.Deserialize<JsonElement>(stream);*/
                //Предлагается кидать stream а с него JSON, но можно все сразу без стрима
                //Так как метод там только асинхрон то придется вставить Wait (хотя лучше еще таймаут добавить..)
                var responseBodyTask = captchaResponse.Content.ReadFromJsonAsync<JsonElement>();
                responseBodyTask.Wait();
                JsonElement responseBody = responseBodyTask.Result; //Теперь у нас есть тело типа (иерархия JSON классов)
                bool captchaResult = responseBody.GetProperty("success").GetBoolean(); //Ищем свойство success и получаем значение (там bool)
                if (captchaResult)
                    return true;
            }
            //Возврат false в других случаях
            //Если запрос не прошел, или прошел, но в JSON что-то не получилось и в конечном итоге success = true не достали
            return false;
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
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var message = await u.MessageRepository.GetMessageById(id);
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(MessageDto postMessage)
        {
            var validateResult = validator.Validate(postMessage);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            var message = mapper.Map<Message>(postMessage);
            message.PostDate = DateTime.Now;
            u.MessageRepository.AddMessage(message);
            await u.SaveAsync();
            return Ok(message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, MessageDto putMessage)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var validateResult = validator.Validate(putMessage);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            var mFromDb = await u.MessageRepository.GetMessageById(id);
            mapper.Map(putMessage, mFromDb);
            await u.SaveAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMessage(int id, MessageDto patchMessage)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var messageFromDb = await u.MessageRepository.GetMessageById(id);
            mapper.Map(patchMessage, messageFromDb);
            await u.SaveAsync();

            return NoContent();

        }

        [HttpPatch("json/{id}")]
        public async Task<IActionResult> PatchMessageJSON(int id, [FromBody] JsonPatchDocument<MessageDto> patchMessage)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var mFromDb = await u.MessageRepository.GetMessageById(id); //достать топик из БД
            var mFromDbDTO = mapper.Map<MessageDto>(mFromDb); //Привести запись из бд к виду DTO
            patchMessage.ApplyTo(mFromDbDTO); //Перекинуть JSON в запись из бд в формате DTO
            //Это важно, поскольку в теле представлен топик DTO, ApplyTo можно применить только к топику DTO
            //Нельзя переписать ApplyTo DTO Topic из запроса в БД топик, так как типы разные!
            mapper.Map(mFromDbDTO, mFromDb); //Смаппить DTO формат обратно в обычный
            await u.SaveAsync(); //Сохраняем обновленную сущность

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var selectedMessage = u.MessageRepository.GetMessageById(id);
            if (selectedMessage == null)
                return BadRequest("Message with current ID does not exist");
            u.MessageRepository.DeleteMessage(id);
            await u.SaveAsync();
            return Ok(id);
        }

        //ДОБАВЛЕНИЕ НОВОГО СООБЩЕНИЯ ИЗ ФОРМЫ ФРОНТА
        [HttpPost("feedback")]
        public async Task<IActionResult> AddMessage(bool captcha, MessagePostDto message)
        {
            //Валидация сообщения как на фронте
            var validateResult = validatorPost.Validate(message);
            if (!validateResult.IsValid)
            {
                //Take Count проверить что в коллекции ошибок только одна
                if (validateResult.Errors.Take(2).Count() == 1)
                {
                    //И если одна и это капча
                    if (validateResult.Errors.First().PropertyName.Equals("captcha"))
                        if (captcha) //И если контроль капчи On то дать отбой клиенту
                            return BadRequest("Capcha can not be empty!");
                    else
                            return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
                }
                else
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            }
            if (captcha)
            {
                if (!CheckCaptcha(message.captcha))
                    return BadRequest("Captcha validation failed");
            }
            //Поиск и проверка что номер топика реальный
            var selectedTopic = u.MessageTopicRepository.GetMessageTopicById(message.topicId);
            if (selectedTopic == null)
                return BadRequest("Topic with current ID does not exist");

            //ищем контакт в бд, если нет то добавим новый
            Contact contact = await u.ContactRepository.GetContactByData(message.contactMail, message.contactPhone);
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
                contact = await u.ContactRepository.GetContactByData(message.contactMail, message.contactPhone);
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
                messageTopic = selectedTopic,
                message = newMessage,
                contact = contact,
            };
            return Ok(db_record);
        }
    }
}
