namespace FeedbackAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IContactRepository ContactRepository { get; }
        IMessageTopicRepository MessageTopicRepository { get; }
        IMessageRepository MessageRepository { get; }   
        Task<bool> SaveAsync();
        bool Save();
    }
}
