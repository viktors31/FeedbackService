using AutoMapper;
using FeedbackAPI.Data;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork u;
        private readonly IMapper mapper;
        private readonly IValidator<ContactDto> validator;
        public ContactController(IUnitOfWork u, IMapper mapper, IValidator<ContactDto> validator) { 
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
        // GET api/contact - All contacts
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contact_list = await u.ContactRepository.GetContacts();
            var contact_list_dto = mapper.Map<IEnumerable<ContactDto>>(contact_list);
            return Ok(contact_list_dto);
        }
        //GET - api/contact/id - Defined contact
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact([FromRoute]int id)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var contactFromDb = await u.ContactRepository.GetContact(id);
            if (contactFromDb == null)
                return BadRequest("Contact with current ID does not exist");
            return Ok(contactFromDb);
        }
        // GET api/contact/bydata - Get contact by Phone + Mail
        [HttpGet("bydata")]
        public async Task<IActionResult> GetContactByData(ContactPhoneMailDto contact)
        {
            var contactFromDb = await u.ContactRepository.GetContactByData(contact.Mail, contact.Phone);
            if (contactFromDb == null)
                return BadRequest("Contact with current mail, phone does not exist");
            return Ok(contactFromDb);
        }

        //POST api/contact - Post new contact in JSON Format
        [HttpPost]
        public async Task<IActionResult> AddContact(ContactDto postContact)
        {
            var validateResult = validator.Validate(postContact);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            var contact = mapper.Map<Contact>(postContact);
            u.ContactRepository.AddContact(contact);
            await u.SaveAsync();
            return Ok(contact);
        }

        //PUT api/contact - Put in existing contact
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, ContactDto putContact)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var validateResult = validator.Validate(putContact);
            if (!validateResult.IsValid)
                return BadRequest(new { error = validateResult.Errors.Select(err => new { err.AttemptedValue, err.ErrorMessage }) });
            var contactFromDb = await u.ContactRepository.GetContact(id);
            if (contactFromDb == null)
                return BadRequest("Contact with current ID does not exist");
            mapper.Map(putContact, contactFromDb);
            await u.SaveAsync();
            return Ok();
        }
        //PATCH api/contact - Classic
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchContact(int id, ContactDto patchContact)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var contactFromDb = await u.ContactRepository.GetContact(id);
            if (contactFromDb == null)
                return BadRequest("Contact with current ID does not exist");
            mapper.Map(patchContact, contactFromDb);
            await u.SaveAsync();

            return NoContent();

        }
        //PATCH api/contact - ASP.NET JSON
        [HttpPatch("json/{id}")]
        public async Task<IActionResult> PatchContactJSON(int id, [FromBody] JsonPatchDocument<ContactDto> patchContact)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var cFromDb = await u.ContactRepository.GetContact(id); //достать топик из БД
            var cFromDbDTO = mapper.Map<ContactDto>(cFromDb); //Привести запись из бд к виду DTO
            patchContact.ApplyTo(cFromDbDTO); //Перекинуть JSON в запись из бд в формате DTO
            //Это важно, поскольку в теле представлен топик DTO, ApplyTo можно применить только к топику DTO
            //Нельзя переписать ApplyTo DTO Topic из запроса в БД топик, так как типы разные!
            mapper.Map(cFromDbDTO, cFromDb); //Смаппить DTO формат обратно в обычный
            await u.SaveAsync(); //Сохраняем обновленную сущность

            return NoContent();

        }

        //DELETE api/contact - Delete contact with current ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (!IdValid(id))
                return BadRequest("ID is invalid");
            var selectedContact = u.ContactRepository.GetContact(id);
            if (selectedContact != null)
            {
                u.ContactRepository.DeleteContact(id);
                await u.SaveAsync();
            }
            else
                return BadRequest("Contact with current ID does not exist");
            return Ok(selectedContact);
        }
    }
}
