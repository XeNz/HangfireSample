using HangfireSample.DataProviders.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireSample.DataProviders
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CommentReadHistory> CommentReadHistories { get; set; }
    }
}