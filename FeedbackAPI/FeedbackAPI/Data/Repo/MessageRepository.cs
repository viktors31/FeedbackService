using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

using System.Numerics;

namespace FeedbackAPI.Data.Repo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly FeedbackDbContext DbContext;

        public MessageRepository(FeedbackDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public void AddMessage(Message message)
        {
            DbContext.Message.AddAsync(message);
        }

        public void DeleteMessage(int id)
        {
            var message = DbContext.Message.Find(id);
            DbContext.Message.Remove(message);
        }

        public async Task<IEnumerable> GetMessages()
        {
            return await DbContext.Message.ToListAsync();
        }

        public async Task<IEnumerable> GetMessagesByContactId(int id)
        {
            var messages = from msg in DbContext.Message
                           where msg.ContactId == id
                           select msg;
            return await messages.ToListAsync();
            
        }

        public async Task<FullView> GetMessageById(int id)
        {
            if (id < 0)
                return null;
            var _message = DbContext.Message.Find(id);
            if (_message == null)
                return null;
            var _messagetopic = DbContext.MessageTopic.Find(_message.TopicId);
            var _contact = DbContext.Contact.Find(_message.ContactId);
            var full = new FullView
            {
                message = _message,
                messagetopic = _messagetopic,
                contact = _contact
            };
            return full;

        }
    }
}
