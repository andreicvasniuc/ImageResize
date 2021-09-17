using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using ImageResize.Models;
using Microsoft.Extensions.Logging;

namespace ImageResize.Services
{
    public class LogStorageService : ILogStorageService
    {
        private readonly TableClient _table;
        private readonly ILogger<LogStorageService> _logger;

        public LogStorageService(string connectionString, ILogger<LogStorageService> logger)
        {
            _logger = logger;
            _table = InstantiateAppLogsTable(connectionString);
        }

        async Task<bool> ILogStorageService.Log(string imageName)
        {
            try
            {
                var entity = new AppLogEntity {
                    PartitionKey = DateTimeOffset.Now.Date.ToLongDateString(),
                    RowKey = imageName,
                    ImageName = imageName,
                    Message = "Create a thumb from image",
                    Timestamp = DateTimeOffset.Now
                };
                var response = await _table.AddEntityAsync(entity);
                return response.Status == 201;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return false;
            }
        }

        private TableClient InstantiateAppLogsTable(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Can't be empty", nameof(connectionString));

                var table = new TableClient(connectionString, tableName: "applogs");
                table.CreateIfNotExists();

                return table;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }
    }
}
