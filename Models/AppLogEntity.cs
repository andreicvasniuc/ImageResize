using Azure;
using Azure.Data.Tables;
using System;

namespace ImageResize.Models
{
    public class AppLogEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ImageName { get; set; }
        public string Message { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
