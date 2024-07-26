using FeedbackAPI.Models;
using System.Collections;

namespace FeedbackAPI.Interfaces
{
    public interface IMessageTopicRepository
    {
        public Task<IEnumerable> GetMessageTopics();
        public MessageTopic GetMessageTopicById(int id);
        public void AddMessageTopic(MessageTopic topic);
        public void DeleteMessageTopic(MessageTopic deletingTopic);
        public void UpdateMessageTopic(MessageTopic topic);
    }
}
