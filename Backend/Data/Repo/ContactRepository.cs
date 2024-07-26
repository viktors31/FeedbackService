using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace FeedbackAPI.Data.Repo
{
    public class ContactRepository : IContactRepository
    {
        private readonly FeedbackDbContext DbContext;

        public ContactRepository(FeedbackDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public void AddContact(Contact contact)
        {
            DbContext.Contact.AddAsync(contact);
        }

        public void DeleteContact(int id)
        {
            var contact = DbContext.Contact.Find(id);
            DbContext.Contact.Remove(contact);
        }

        public async Task<IEnumerable> GetContacts()
        {
            return await DbContext.Contact.ToListAsync();
        }

        public async Task<Contact> GetContact(int id)
        {
            return await DbContext.Contact.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Contact> GetContactByData(string mail, string phone)
        {
 
            return await DbContext.Contact
                .Where(c => c.Mail == mail && c.Phone == phone)
                .FirstOrDefaultAsync();
        }
    }
}
