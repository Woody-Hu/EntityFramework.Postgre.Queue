using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Postgre.Queue
{
    public class QueueHandler
    {
        public async Task<QueueItem> EnqueueAsync(QueueDbContext context, string value)
        {
            var item = new QueueItem() { Id = Guid.NewGuid().ToString(), Value = value, State = 0, LastModifyDateTime = DateTime.UtcNow };
            var res = await context.Queue.AddAsync(item);
            return res.Entity;
        }

        public async Task<QueueItem> DequeueAsync(QueueDbContext context)
        {
            var now = DateTime.UtcNow;
            var limitTime = now - TimeSpan.FromSeconds(5);
            var res = await context.Queue
                .FromSql("UPDATE \"Queue\" SET \"State\" = 1, \"LastModifyDateTime\" = {0} WHERE \"Id\" = (SELECT \"Id\" FROM \"Queue\" WHERE \"State\" = 0 OR (\"State\" = 1 AND \"LastModifyDateTime\" < {1} ) ORDER BY \"LastModifyDateTime\" FOR UPDATE SKIP LOCKED LIMIT 1) RETURNING *", now, limitTime)
                .ToListAsync();
            var item = res.FirstOrDefault();
            if (item != null)
            {
                context.Queue.Remove(item);
            }
            return item;
        }
    }
}
