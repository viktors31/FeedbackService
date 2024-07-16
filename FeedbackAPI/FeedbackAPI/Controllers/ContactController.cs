using AutoMapper;
using FeedbackAPI.Data;
using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.AspNetCore.Http;
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
        public ContactController(IUnitOfWork u, IMapper mapper) { 
            this.u = u;
            this.mapper = mapper;
        }
        // GET api/contact - All contacts
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contact_list = await u.ContactRepository.GetContacts();
            var contact_list_dto = mapper.Map<IEnumerable<ContactDto>>(contact_list);
            return Ok(contact_list_dto);
        }

        // GET api/contact/bydata - Get contact by Phone + Mail
        [HttpGet("/bydata")]
        public async Task<IActionResult> GetContactByData(ContactPhoneMailDto contact)
        {
            var contactId = u.ContactRepository.GetContactByData(contact.Mail, contact.Phone);
            return Ok(contactId);
        }

        //POST api/contact - Post new contact in JSON Format
        [HttpPost]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            u.ContactRepository.AddContact(contact);
            await u.SaveAsync();
            return Ok(contact);
        }

        //DELETE api/contact - Delete contact with current ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            u.ContactRepository.DeleteContact(id);
            await u.SaveAsync();
            return Ok(id);
        }
    }
}
