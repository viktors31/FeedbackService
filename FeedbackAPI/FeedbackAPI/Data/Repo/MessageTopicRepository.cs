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
            DbContext.MessageTopic.AddAsync(topic);
        }

        public void DeleteMessageTopic(string topicName)
        {
            var deletingTopic = DbContext.MessageTopic.Where(t => t.Name == topicName).FirstOrDefault();
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

    }
}
