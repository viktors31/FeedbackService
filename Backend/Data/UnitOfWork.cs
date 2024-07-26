using FeedbackAPI.Data.Repo;
using FeedbackAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeedbackAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FeedbackDbContext DbContext;
        public UnitOfWork(FeedbackDbContext DbContext) 
        {
            this.DbContext = DbContext;
        }
        public IContactRepository ContactRepository => new ContactRepository(DbContext);
        public IMessageTopicRepository MessageTopicRepository => new MessageTopicRepository(DbContext);
        public IMessageRepository MessageRepository => new MessageRepository(DbContext);

        public async Task<bool> SaveAsync()
        {
            return await DbContext.SaveChangesAsync() > 0;
        }
        public bool Save()
        {
            return DbContext.SaveChanges() > 0;
        }
    }
}
