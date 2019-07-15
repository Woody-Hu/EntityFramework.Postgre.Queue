using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Postgre.Queue
{
    public class QueueDbContext : DbContext
    {
        public QueueDbContext(DbContextOptions<QueueDbContext> options) : base(options)
        {

        }

        public DbSet<QueueItem> Queue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<QueueItem>().HasKey(k => k.Id);
            modelBuilder.Entity<QueueItem>().Property(k => k.Id).IsUnicode();
            modelBuilder.Entity<QueueItem>().ForNpgsqlHasIndex(k => k.Id);
            modelBuilder.Entity<QueueItem>().Property(k => k.LastModifyDateTime).IsConcurrencyToken();
        }
    }
}
