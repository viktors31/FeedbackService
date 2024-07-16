using FeedbackAPI.Models;
using FeedbackAPI.Dto;
using System.Collections;

namespace FeedbackAPI.Interfaces
{
    public interface IMessageRepository
    {
        public Task<IEnumerable> GetMessages();
        public void AddMessage(Message message);
        public void DeleteMessage(int id);
        public Task<IEnumerable> GetMessagesByContactId(int id);
        public Task<FullView> GetMessageById(int id);
    }
}
