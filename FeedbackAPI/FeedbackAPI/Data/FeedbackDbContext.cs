using Microsoft.EntityFrameworkCore;
using FeedbackAPI.Models;

namespace FeedbackAPI.Data
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options) { }

        public DbSet<Contact> Contact {  get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageTopic> MessageTopic { get; set; }

    }
}
