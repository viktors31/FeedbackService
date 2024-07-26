using FeedbackAPI.Models;
using System.Collections;

namespace FeedbackAPI.Interfaces
{
    public interface IContactRepository
    {
        public Task<IEnumerable> GetContacts();
        public void AddContact(Contact contact);
        public void DeleteContact(int id);
        public Task<Contact> GetContact(int id);
        public Task<Contact> GetContactByData(string name, string phone);
    }
}
