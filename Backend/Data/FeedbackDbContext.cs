using Microsoft.EntityFrameworkCore;
using FeedbackAPI.Models;
using FeedbackAPI.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FeedbackAPI.Data
{
    public class FeedbackDbContext : IdentityDbContext<User, IdentityRole<long>, long>/*DbContext*/
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options) {
            //Database.Migrate();
        }

        public DbSet<Contact> Contact {  get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageTopic> MessageTopic { get; set; }
        public DbSet<User> Users { get; set; } = null!;

    }
}
