using System;

namespace EntityFramework.Postgre.Queue
{
    public class QueueItem
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public int State { get; set; }

        public DateTime LastModifyDateTime { get; set; }
    }
}
