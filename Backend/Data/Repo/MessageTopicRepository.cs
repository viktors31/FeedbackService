using FeedbackAPI.Dto;
using FeedbackAPI.Interfaces;
using FeedbackAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace FeedbackAPI.Data.Repo
{
    public class MessageTopicRepository : IMessageTopicRepository
    {
        private readonly FeedbackDbContext DbContext;
        public MessageTopicRepository(FeedbackDbContext DbContext) 
        {
            this.DbContext = DbContext;
        } 
        public void AddMessageTopic(MessageTopic topic)
        {
            DbContext.MessageTopic.Add(topic);
        }

        public void DeleteMessageTopic(MessageTopic deletingTopic)
        {
            DbContext.MessageTopic.Remove(deletingTopic);
        }

        public async Task<IEnumerable> GetMessageTopics()
        {
            return await DbContext.MessageTopic.ToListAsync();
        }

        public MessageTopic GetMessageTopicById(int id)
        {
            return DbContext.MessageTopic.Where(t => t.Id == id).FirstOrDefault();
        }

        public void UpdateMessageTopic(MessageTopic topic)
        {
            DbContext.MessageTopic.Update(topic);

        }
    }
}
